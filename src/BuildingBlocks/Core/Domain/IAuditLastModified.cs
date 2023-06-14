namespace BuildingBlocks.Core.Domain
{
    public interface IAuditLastModified
    {
        public DateTimeOffset LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
