namespace Fairway.Core.Data.Sql.Base.Interface
{
    public interface INeedDataAuditTracking
    {
        long DataTrackingKey { get; }

        int Classification { get; }
    }
}
