using System;

namespace Fairway.Core.Data.Sql.Base.Interface
{
    public interface IRecordModification
    {
        DateTime ModifiedOn { get; set; }
        int ModifiedByUserId { get; set; }
    }
}
