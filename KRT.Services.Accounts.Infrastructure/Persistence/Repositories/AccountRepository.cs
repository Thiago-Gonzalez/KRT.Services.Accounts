using KRT.Services.Accounts.Core.Entities;
using KRT.Services.Accounts.Core.Exceptions;
using KRT.Services.Accounts.Core.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace KRT.Services.Accounts.Infrastructure.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly KRTAccountsDbContext _context;

    public AccountRepository(KRTAccountsDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByCpfAsync(string cpf) => await _context.Accounts .IgnoreQueryFilters().AnyAsync(a => a.Cpf.Value == cpf);

    public async Task AddAsync(Account account)
    {
        await _context.AddAsync(account);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Account>> GetAllAsync() => await _context.Accounts.ToListAsync();

    public async Task<Account?> GetByIdAsync(int id) => await _context.Accounts.SingleOrDefaultAsync(a => a.Id == id);

    public async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx &&
                                    (sqlEx.Number == 2601 || sqlEx.Number == 2627) &&
                                    sqlEx.Message.Contains("IX_Accounts_Cpf"))
        {
            throw new BusinessRuleValidationException("Já existe uma conta cadastrada para o CPF informado.", ex);
        }
    }
}
