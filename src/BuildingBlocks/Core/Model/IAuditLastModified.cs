namespace BuildingBlocks.Core.Model
{
    public interface IAuditLastModified
    {
        public DateTimeOffset LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
