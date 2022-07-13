using Fairway.Core.Data.Sql.Base.Models;
using System.Collections.Generic;

namespace Fairway.Core.Data.Sql.Base.Interface
{
    public interface IPaginationSetting
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        IEnumerable<OrderBySetting> OrderBySettings { get; set; }
        int? RecordCount { get; set; }
        int? PageCount { get; }
    }
}
