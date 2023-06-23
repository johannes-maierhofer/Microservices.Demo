namespace BuildingBlocks.HealthCheck;

public class HealthOptions
{
    public bool Enabled { get; set; } = true;
    public string SqlServerStorageConnectionString { get; set; } = string.Empty;
}
