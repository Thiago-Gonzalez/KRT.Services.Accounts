using KRT.Services.Accounts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace KRT.Services.Accounts.Infrastructure.Persistence;

public class KRTAccountsDbContext : DbContext
{
    public KRTAccountsDbContext(DbContextOptions<KRTAccountsDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
