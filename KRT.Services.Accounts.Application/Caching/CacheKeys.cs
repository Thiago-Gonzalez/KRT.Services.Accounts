namespace KRT.Services.Accounts.Application.Caching;

public static class CacheKeys
{
    public static string AccountById(int id) => $"accounts:{id}";
    public const string AccountsList = "accounts:all";
}
