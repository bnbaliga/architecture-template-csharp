using Fairway.Core.Data.Sql.EF.Concrete;
using Fairway.Core.Data.Sql.Base.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Transactions;
using Fairway.Core.Data.Sql.Base.Models;

namespace Fairway.Core.Data.Sql.EF
{
    public abstract class DbContextBase : DbContext, IDbContext, IDisposable
    {
        private readonly int _currentUserId;        
        private readonly bool _toUpdate;
        private readonly string _connString;

        #region Init

        private void Init()
        {
           ChangeTracker.AutoDetectChangesEnabled = false;
        }

        #endregion

        #region Constructors      

        protected DbContextBase(int currentUserId, string nameOrConnectionString, bool toUpdate, bool useManagedIdentity = true)
        {
            _connString = nameOrConnectionString;
            _currentUserId = currentUserId;
            _toUpdate = toUpdate;

            Init();

            if (useManagedIdentity)
                (Database.GetDbConnection() as SqlConnection).AccessToken = AzureHelper.GetAccessToken().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        //This method is called after DbContext is created for configuration.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.DetachedLazyLoadingWarning))
                .UseSqlServer(_connString);
        }

        #endregion

        #region CRUD Operations

        public IQueryable<T> Get<T>(
            Expression<Func<T, bool>> wherePredicate, 
            IEnumerable<string> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            ) where T : DbEntityBase
        {
            var query = wherePredicate == null ?
                            Set<T>().AsQueryable() :
                            Set<T>().Where(wherePredicate).AsQueryable();

            if (includes != null)
            {
                foreach (var navItem in includes)
                    query = query.Include(navItem);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return _toUpdate ? query : query.AsNoTracking();
        }

        public T Find<T>(object id) where T: DbEntityBase
        {
            if (_toUpdate)
            {
                return Set<T>().Find(id);
            }
            else
            {
                var entity = Set<T>().Find(id);
                if(entity != null) 
                    Entry(entity).State = EntityState.Detached;
                return entity;
            }
        }

        public T Create<T>() where T : DbEntityBase, new()
        {
            return Set<T>().Add(new T()).Entity;
        }

        public T AttachToContext<T>(T entity) where T : DbEntityBase
        {
            return Set<T>().Attach(entity).Entity;
        }

        public void Delete<T>(T entity) where T : DbEntityBase
        {
            if (entity != null)
                Set<T>().Remove(entity);
        }

        public IEnumerable<T> Delete<T>(IEnumerable<T> entities) where T : DbEntityBase
        {
            if (entities != null) Set<T>().RemoveRange(entities);
            return entities;
        }

        public void Delete<T>(Expression<Func<T, bool>> wherePredicate) where T : DbEntityBase
        {
            var entities = Set<T>().Where(wherePredicate).AsEnumerable();
            Delete(entities);
        }

        public int Save()
        {
            if (!HasChanges()) return 0;

            var result = 0;

            var changedEntries = this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Modified || p.State == EntityState.Deleted).ToList();

            var addedEntries = changedEntries.Where(p => p.State == EntityState.Added).ToList();
            var modifiedEntries = changedEntries.Where(p => p.State == EntityState.Modified).ToList();
            var deletedEntries = changedEntries.Where(p => p.State == EntityState.Deleted).ToList();
            
            //data tracking
            var dataTrackingEntriesEnumerator = CollectDataTrackingEntries(EntityState.Modified, modifiedEntries);
            dataTrackingEntriesEnumerator = dataTrackingEntriesEnumerator.Concat(CollectDataTrackingEntries(EntityState.Deleted, deletedEntries));
            var dataTrackingEntries = dataTrackingEntriesEnumerator?.ToArray();
            //Record the timestamp and user for the newly created and modified entries
            RecordCreations(addedEntries);
            RecordModifications(modifiedEntries);

            using (var transScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                result = SaveData();
                                
                dataTrackingEntries = dataTrackingEntries.Concat(CollectDataTrackingEntries(EntityState.Added, addedEntries))?.ToArray();

                transScope.Complete();
            }
            
            //Call Save one more time to update the PostSave entries
            Save();

            try
            {
                SubmitDataTrackingEntries(dataTrackingEntries);
            }
            catch (System.Exception)
            {
                // ignored
            }

            return result;
        }

        
        public void RollBack()
        {
            if (!HasChanges()) return;

            IList<EntityEntry> changedEntries = this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Modified || p.State == EntityState.Deleted).ToList();

            IList<EntityEntry> modifiedEntries = changedEntries.Where(p => p.State == EntityState.Modified).ToList();
            IList<EntityEntry> addedEntries = changedEntries.Where(p => p.State == EntityState.Added).ToList();
            IList<EntityEntry> deletedEntries = changedEntries.Where(p => p.State == EntityState.Deleted).ToList();

            foreach (var entry in modifiedEntries)
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }

            foreach (var entry in addedEntries)
            {
                entry.State = EntityState.Detached;
            }

            foreach (var entry in deletedEntries)
            {
                entry.State = EntityState.Unchanged;
            }
        }

        #endregion

        #region IDbContext Query Methods

        DateTime IDbContext.GetDbCurrentDateTime()
        {
            var _currDate = new SqlParameter("currDate", "")
            {
                Direction = ParameterDirection.Output,
                DbType = DbType.DateTime                
            };

            var sql = "Select @currDate = GetDate()";

            var dr = this.Database.ExecuteSqlRaw(sql, _currDate);

            return (DateTime)_currDate.Value; 
        }

        int IDbContext.ExecuteSqlCommand(string command, params Object[] parameters)
        {
            return Database.ExecuteSqlRaw(command, parameters);
        }       

        public IQueryable<T> ExecuteQuery<T>(string query, params Object[] parameters) where T : DbEntityBase
        {
            return Set<T>().FromSqlRaw(query,parameters);
        }

        public bool IsEntityLoaded<TParent>(DbEntityBase parentEntity, string navigationalPropertyName) where TParent : DbEntityBase
        {
            if (string.IsNullOrWhiteSpace(navigationalPropertyName))
                throw new ArgumentNullException(nameof(navigationalPropertyName));

            if (parentEntity == null)
                throw new ArgumentNullException(nameof(parentEntity));

            if (base.ChangeTracker.Entries().All(w => w.Entity != parentEntity))
                throw new InvalidOperationException("Tracking not enabled, please make sure that 'toUpdate' is set to true.");

            var isLoaded = false;
            try
            {
                if (Entry((TParent) parentEntity).Member(navigationalPropertyName) is ReferenceEntry)
                {
                    isLoaded = Entry((TParent) parentEntity).Reference(navigationalPropertyName).IsLoaded;
                }
                if (Entry((TParent) parentEntity).Member(navigationalPropertyName) is CollectionEntry)
                {
                    isLoaded = Entry((TParent) parentEntity).Collection(navigationalPropertyName).IsLoaded;
                }
            }
            catch (ArgumentException exception)
            {
                throw new InvalidOperationException("Invalid property name: " + exception.Message);
            }

            return isLoaded;
        }

        #endregion

        #region Private Methods

        //private ObjectContext ObjectContext => ((IObjectContextAdapter) this).ObjectContext;

        private bool HasChanges()
        {
            ChangeTracker.DetectChanges();
            return ((from ent in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == (EntityState) EntityState.Deleted || p.State == EntityState.Modified) select ent).Any());
        }

        private void RecordCreations(IList<EntityEntry> entries)
        {
            foreach (var entry in entries.Where(w => w.Entity is IRecordCreation))
            {
                ((IRecordCreation) entry.Entity).CreatedOn = ((IDbContext) this).GetDbCurrentDateTime();
                ((IRecordCreation)entry.Entity).CreatedByUserId = _currentUserId;
            }
            RecordModifications(entries);
        }

        private void RecordModifications(IList<EntityEntry> entries)
        {
            foreach (var entry in entries.Where(w => w.Entity is IRecordModification))
            {
                ((IRecordModification) entry.Entity).ModifiedOn = ((IDbContext) this).GetDbCurrentDateTime();
                ((IRecordModification)entry.Entity).ModifiedByUserId = _currentUserId;
            }
        }

        //Todo: Configure # of save attempts
        private int SaveData()
        {
            bool saveFailed;
            var saveReturn = 0;
            do
            {
                saveFailed = false;
                try
                {
                    saveReturn = base.SaveChanges();
                }
                //catch (DbEntityValidationException ex)
                //{
                //    var errorList = new StringBuilder();

                //    foreach (var validationResult in ex.EntityValidationErrors.ToList())
                //        foreach (var validationError in validationResult.ValidationErrors.ToList())
                //            errorList.AppendLine(validationError.PropertyName + " : " + validationError.ErrorMessage);

                //    throw new System.Exception("Save failed!\n" + errorList.ToString());
                //}               
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update original values from the dataUtil, this is the client wins pattern
                    var entry = ex.Entries.Single();
                    entry?.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
                catch (DbUpdateException ex)
                {
                    var errorList = new StringBuilder();
                    foreach (var key in ex.Data)
                        errorList.AppendLine(key + " : " + ex.Data[key]);

                    throw new System.Exception("Save failed! \nMessage: " + ex.Message.ToString() +  " \nInner Exception: "  + (ex.InnerException != null ? ex.InnerException.Message: null));
                }
            } while (saveFailed);

            return saveReturn;
        }

        #endregion

        #region Data Tracking Methods

        private IEnumerable<DataTrackingEntry> CollectDataTrackingEntries(EntityState forState, IEnumerable<EntityEntry> entries)
        {
            var trackingEntries = GetDataTrackingEntries(entries)?.ToList();
            if (trackingEntries == null || !trackingEntries.Any()) yield break;
            foreach (var entry in trackingEntries)
            {
                Type entityType;
                if (entry.Entity.GetType().BaseType != typeof(DbEntityBase) && entry.Entity.GetType().Namespace == "Castle.Proxies")
                    entityType = entry.Entity.GetType().BaseType;
                else
                    entityType = entry.Entity.GetType();

                if (!(entry.Entity is INeedDataAuditTracking))
                    throw new InvalidOperationException($"Entity {entityType} doesn't implement INeedDataAuditTracking interface.");

                //var metadataItems = ObjectContext.MetadataWorkspace.GetItems<EntityContainer>(DataSpace.SSpace)
                //        .First()
                //        .BaseEntitySets.First(meta => meta.Name == entityType.Name);

                //var tableName = metadataItems.MetadataProperties["Schema"].Value + "." +
                //                metadataItems.MetadataProperties["Name"].Value;


                var metadataItem = this.Model.FindEntityType(entityType);
                var tableName = metadataItem.GetSchema() + "." + metadataItem.GetTableName();

                //get the ID which will be used as context in audit tracking entry 
                var recordId = ((INeedDataAuditTracking) entry.Entity).DataTrackingKey;
                if (recordId == 0)
                    throw new InvalidOperationException($"Entity {entityType} doesn't have the valid data tracking key");

                //get the classification which will be used as context in audit tracking entry 
                var classification = ((INeedDataAuditTracking) entry.Entity).Classification;
                if (classification == 0)
                    throw new InvalidOperationException($"Entity {entityType} doesn't have the valid data tracking classification");

                var entryValues = entry.State == EntityState.Deleted ? entry.OriginalValues : entry.CurrentValues;

                foreach (var property in entryValues.Properties)
                {
                    string columnName = property.Name;
                    var originalValueObj = entry.OriginalValues?[columnName];
                    var newValueObj = entry.State == EntityState.Deleted ? null : entry.CurrentValues?[columnName];
                    //if the data type is array then we don't want to insert audit tracking
                    if (originalValueObj != null &&
                        entry.OriginalValues[columnName].GetType().IsArray
                        ||
                        (newValueObj != null && entry.CurrentValues[columnName].GetType().IsArray))
                        continue;
                    switch (forState)
                    {
                        case EntityState.Modified:
                            if (!object.Equals(originalValueObj, newValueObj))
                            {
                                yield return
                                    new DataTrackingEntry(_currentUserId, classification,
                                        tableName, columnName, recordId, originalValueObj?.ToString(),
                                        newValueObj?.ToString());
                            }
                            break;
                        case EntityState.Added:
                            {
                                yield return
                                    new DataTrackingEntry(_currentUserId, classification,
                                        tableName, columnName, recordId, string.Empty, newValueObj?.ToString());
                            }
                            break;
                        case EntityState.Deleted:
                            {
                                yield return
                                    new DataTrackingEntry(_currentUserId, classification,
                                        tableName, columnName, recordId, originalValueObj?.ToString(), "[DEL]");
                            }
                            break;
                        case EntityState.Detached:
                            break;
                        case EntityState.Unchanged:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(forState), forState, null);
                    }
                }
            }
        }

        protected abstract void SubmitDataTrackingEntries(DataTrackingEntry[] dataTrackingEntries);

        protected abstract IEnumerable<EntityEntry> GetDataTrackingEntries(IEnumerable<EntityEntry> entries);

        #endregion

        void IDisposable.Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
