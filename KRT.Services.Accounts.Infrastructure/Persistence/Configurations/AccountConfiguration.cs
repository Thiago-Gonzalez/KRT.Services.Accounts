using KRT.Services.Accounts.Core.Entities;
using KRT.Services.Accounts.Core.Enums;
using KRT.Services.Accounts.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KRT.Services.Accounts.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);

        builder
            .Property(a => a.HolderName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(a => a.Cpf)
            .HasColumnName("Cpf")
            .HasConversion(
                cpf => cpf.Value,
                value => Cpf.Create(value)
            )
            .HasMaxLength(11)
            .IsRequired();

        builder
            .Property(a => a.Status)
            .IsRequired();

        builder
            .HasIndex(a => a.Cpf)
            .IsUnique()
            .HasDatabaseName("IX_Accounts_Cpf");

        builder.HasQueryFilter(a => a.Status == AccountStatusEnum.Active);
    }
}
