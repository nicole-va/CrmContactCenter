using CrmContactCenter.Server.Data;
using CrmContactCenter.Server.DTOs.Accounts;
using CrmContactCenter.Server.Models;
using CrmContactCenter.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CrmContactCenter.Server.Models.Enums;


namespace CrmContactCenter.Server.Services
{
    public class AccountService: IAccountService
    {
        private readonly CrmDbContext _context;

        public AccountService(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountDto>> GetByCustomerAsync(int customerId)
        {
            return await _context.Accounts
                .Include(a => a.Customer)
                .Where(a => a.CustomerId == customerId)
                .Select(a => ToDto(a))
                .ToListAsync();
        }

        public async Task<AccountDto?> GetByIdAsync(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.Id == id);

            return account == null ? null : ToDto(account);
        }

        public async Task<AccountDto> CreateAsync(CreateAccountDto dto)
        {
            var account = new Account
            {
                CustomerId = dto.CustomerId,
                CreatedBy = dto.CreatedBy,
                AccountNumber = dto.AccountNumber,
                Description = dto.Description,
                Amount = dto.Amount,
                DueDate = dto.DueDate,
                Currency = dto.Currency,
                Notes = dto.Notes,
                Status = AccountStatus.pendiente,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            await _context.Entry(account).Reference(a => a.Customer).LoadAsync();
            return ToDto(account);
        }

        public async Task<AccountDto?> UpdateAsync(int id, UpdateAccountDto dto)
        {
            var account = await _context.Accounts
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null) return null;

            account.Description = dto.Description;
            account.Amount = dto.Amount;
            account.DueDate = dto.DueDate;
            account.Status = Enum.Parse<AccountStatus>(dto.Status);
            account.Notes = dto.Notes;
            account.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return ToDto(account);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return false;

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        private static AccountDto ToDto(Account a) => new AccountDto
        {
            Id = a.Id,
            CustomerId = a.CustomerId,
            CustomerName = a.Customer != null ? $"{a.Customer.FirstName} {a.Customer.LastName}" : string.Empty,
            AccountNumber = a.AccountNumber,
            Description = a.Description,
            Amount = a.Amount,
            DueDate = a.DueDate,
            Status = a.Status.ToString(),
            Currency = a.Currency,
            Notes = a.Notes,
            CreatedAt = a.CreatedAt
    
        };
}
}
