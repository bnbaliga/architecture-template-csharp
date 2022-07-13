using Fairway.Core.Data.Sql.Base.Enums;
using System;

namespace Fairway.Core.Data.Sql.Base.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class OrderByAttribute : System.Attribute
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string NavigationRoute { get; set; }
        public bool IsDefault { get; set; } = false;
        public OrderByDirection DefaultOrder { get; set; } = OrderByDirection.Asc;
    }
}
