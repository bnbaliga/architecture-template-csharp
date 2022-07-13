using Fairway.Core.Data.Sql.Base.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fairway.Core.Data.Sql.Base.Models
{
    public class PaginationSetting : IPaginationSetting
    {
        public PaginationSetting(int pageNumber, int pageSize, IEnumerable<OrderBySetting> orderBySettings)
        {
            if (pageNumber == 0)
                throw new ArgumentException($"The '{nameof(pageNumber)}'  parameter was null -- must pass in a valid mapping object");

            if (pageSize == 0)
                throw new ArgumentException($"The '{nameof(pageSize)}' parameter was null -- must pass in a valid mapping object");

            var sortSettings = orderBySettings as OrderBySetting[] ?? orderBySettings.ToArray();
            if (orderBySettings == null)
                throw new ArgumentNullException(nameof(orderBySettings));
            else
                if (!sortSettings.Any())
                throw new ArgumentException("Sort settings cannot be empty for pagination");

            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBySettings = sortSettings;
        }
                
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<OrderBySetting> OrderBySettings { get; set; }
        public int? RecordCount { get; set; }
        public int? PageCount
        {
            get
            {
                int? returnValue = null;

                if (RecordCount != null && RecordCount.Value != 0)
                    returnValue = Convert.ToInt32((RecordCount.Value - 1) / PageSize) + 1;

                return returnValue;
            }
            private set { }
        }
    }
}
