using Fairway.Core.Data.Sql.Base.Enums;

namespace Fairway.Core.Data.Sql.Base.Models
{
    public class OrderBySetting
    {   
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string NavigationRoute { get; set; }
        public OrderByDirection Direction { get; set; } = OrderByDirection.Asc;
    }
}
