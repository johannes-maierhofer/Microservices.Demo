namespace BuildingBlocks.Core.Model
{
    public interface IAuditCreated
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
