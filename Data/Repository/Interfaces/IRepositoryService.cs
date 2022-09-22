using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Data.Repository.Interfaces
{
    public interface IRepositoryService
    {
        //Task<Account> AuthenticateAsync(string AccountNumber, string Pin);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> CreateAsync(Account account, string Pin, string ConfirmPin);
        Task<bool> UpdateAsync(Account account, string Pin = null);
        Task<bool> DeleteAsync(int Id);
        Account GetByIdAsync(int Id);
        Task<Account> GetByAccountNumberAsync(string AccountNumber);
        Task<bool> SaveAsync();
    }
}
