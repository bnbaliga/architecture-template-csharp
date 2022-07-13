using System;

namespace Fairway.Core.Data.Sql.Base.Interface
{
    public interface IRecordCreation
    {
        DateTime CreatedOn { get; set; }
        int CreatedByUserId { get; set; }
    }
}
