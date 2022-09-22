using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace BusinessLogic.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Account> AuthenticateAsync(string AccountNumber, string Pin);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> CreateAsync(Account account, string Pin, string ConfirmPin);
        Task UpdateAsync(Account account, string Pin = null);
        Task DeleteAsync(int Id);
        Task<Account> GetByIdAsync(int Id);
        Task<Account> GetByAccountNumberAsync(string AccountNumber);
        //Task<bool> SaveAsync();
    }
}
