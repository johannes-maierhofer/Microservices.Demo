namespace BuildingBlocks.Core.Domain
{
    public class AuditableEntity<T> : Entity<T>
        where T : struct
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
    }
}
