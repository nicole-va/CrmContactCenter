using CrmContactCenter.Server.DTOs.Accounts;

namespace CrmContactCenter.Server.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountDto>> GetByCustomerAsync(int customerId);
        Task<AccountDto?> GetByIdAsync(int id);
        Task<AccountDto> CreateAsync(CreateAccountDto dto);
        Task<AccountDto?> UpdateAsync(int id, UpdateAccountDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
