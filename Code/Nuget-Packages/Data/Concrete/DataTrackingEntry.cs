namespace Fairway.Core.Data.Sql.EF.Concrete
{
    public class DataTrackingEntry
    {
        public int UserId;
        public int Classification;
        public string TableName;
        public string FieldName;
        public long Context;
        public string PreviousValue;
        public string NewValue;

        public DataTrackingEntry(int userId, int classification, string tableName, string fieldName, long context, string previousValue, string newValue)
        {
            UserId = userId;
            Classification = classification;
            TableName = tableName;
            FieldName = fieldName;
            Context = context;
            PreviousValue = previousValue;
            NewValue = newValue;
        }
    }
}