// Licence file C:\Users\narasimha.baliga\OneDrive - Fairway Independent Mortgage Corporation\Documents\ReversePOCO.txt not found.
// Please obtain your licence file at www.ReversePOCO.co.uk, and place it in your documents folder shown above.
// Defaulting to Trial version.


// ------------------------------------------------------------------------------------------------
// WARNING: Failed to load provider "System.Data.SqlClient" - A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: SQL Network Interfaces, error: 25 - Connection string is not valid)
// Allowed providers:
//    "System.Data.Odbc"
//    "System.Data.OleDb"
//    "System.Data.OracleClient"
//    "System.Data.SqlClient"
//    "Microsoft.SqlServerCe.Client.4.0"

/*   at System.Data.SqlClient.SqlInternalConnectionTds..ctor(DbConnectionPoolIdentity identity, SqlConnectionString connectionOptions, SqlCredential credential, Object providerInfo, String newPassword, SecureString newSecurePassword, Boolean redirectedUserInstance, SqlConnectionString userConnectionOptions, SessionData reconnectSessionData, DbConnectionPool pool, String accessToken, Boolean applyTransientFaultHandling, SqlAuthenticationProviderManager sqlAuthProviderManager)
   at System.Data.SqlClient.SqlConnectionFactory.CreateConnection(DbConnectionOptions options, DbConnectionPoolKey poolKey, Object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection, DbConnectionOptions userOptions)
   at System.Data.ProviderBase.DbConnectionFactory.CreatePooledConnection(DbConnectionPool pool, DbConnection owningObject, DbConnectionOptions options, DbConnectionPoolKey poolKey, DbConnectionOptions userOptions)
   at System.Data.ProviderBase.DbConnectionPool.CreateObject(DbConnection owningObject, DbConnectionOptions userOptions, DbConnectionInternal oldConnection)
   at System.Data.ProviderBase.DbConnectionPool.UserCreateRequest(DbConnection owningObject, DbConnectionOptions userOptions, DbConnectionInternal oldConnection)
   at System.Data.ProviderBase.DbConnectionPool.TryGetConnection(DbConnection owningObject, UInt32 waitForMultipleObjectsTimeout, Boolean allowCreate, Boolean onlyOneCheckConnection, DbConnectionOptions userOptions, DbConnectionInternal& connection)
   at System.Data.ProviderBase.DbConnectionPool.TryGetConnection(DbConnection owningObject, TaskCompletionSource`1 retry, DbConnectionOptions userOptions, DbConnectionInternal& connection)
   at System.Data.ProviderBase.DbConnectionFactory.TryGetConnection(DbConnection owningConnection, TaskCompletionSource`1 retry, DbConnectionOptions userOptions, DbConnectionInternal oldConnection, DbConnectionInternal& connection)
   at System.Data.ProviderBase.DbConnectionInternal.TryOpenConnectionInternal(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource`1 retry, DbConnectionOptions userOptions)
   at System.Data.SqlClient.SqlConnection.TryOpenInner(TaskCompletionSource`1 retry)
   at System.Data.SqlClient.SqlConnection.TryOpen(TaskCompletionSource`1 retry)
   at System.Data.SqlClient.SqlConnection.Open()
   at Microsoft.VisualStudio.TextTemplating3A618BE10DAED70706830BBBB2DC9FE560EA3931DEB08C306CCF354164A5AE020C081D53FC490A75263645DCB254C12DAC9A4F1E6FE7A46188D085457950F19E.GeneratedTextTransformation.DatabaseReader.Init() in C:\BitBucket\VSProjects\Vertical-Product-Service\Code\Vertical.Product.Service.Api\Data\EF.Reverse.POCO.v3.ttinclude:line 12212
   at Microsoft.VisualStudio.TextTemplating3A618BE10DAED70706830BBBB2DC9FE560EA3931DEB08C306CCF354164A5AE020C081D53FC490A75263645DCB254C12DAC9A4F1E6FE7A46188D085457950F19E.GeneratedTextTransformation.SqlServerDatabaseReader.Init() in C:\BitBucket\VSProjects\Vertical-Product-Service\Code\Vertical.Product.Service.Api\Data\EF.Reverse.POCO.v3.ttinclude:line 15616
   at Microsoft.VisualStudio.TextTemplating3A618BE10DAED70706830BBBB2DC9FE560EA3931DEB08C306CCF354164A5AE020C081D53FC490A75263645DCB254C12DAC9A4F1E6FE7A46188D085457950F19E.GeneratedTextTransformation.Generator.Init(DatabaseReader databaseReader, String singleDbContextSubNamespace) in C:\BitBucket\VSProjects\Vertical-Product-Service\Code\Vertical.Product.Service.Api\Data\EF.Reverse.POCO.v3.ttinclude:line 4141
   at Microsoft.VisualStudio.TextTemplating3A618BE10DAED70706830BBBB2DC9FE560EA3931DEB08C306CCF354164A5AE020C081D53FC490A75263645DCB254C12DAC9A4F1E6FE7A46188D085457950F19E.GeneratedTextTransformation.GeneratorFactory.Create(FileManagementService fileManagementService, Type fileManagerType, String singleDbContextSubNamespace) in C:\BitBucket\VSProjects\Vertical-Product-Service\Code\Vertical.Product.Service.Api\Data\EF.Reverse.POCO.v3.ttinclude:line 6293*/
// ------------------------------------------------------------------------------------------------

