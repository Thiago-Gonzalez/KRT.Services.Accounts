namespace KRT.Services.Accounts.Infrastructure.CacheStorage;

public record RedisOptions
{
    public string InstanceName { get; set; } = string.Empty;
    public string Configuration { get; set; } = string.Empty;
}
