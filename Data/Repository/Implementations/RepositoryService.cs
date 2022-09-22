using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Data.Repository.Interfaces;
using Domain.Models;

namespace Data.Repository.Implementations
{
    public class RepositoryService : IRepositoryService
    {
        private AppDbContext _context;

        public RepositoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Account> AuthenticateAsync(string AccountNumber, string Pin)
        {
            await _context..AddAsync(country);
            return await SaveAsync();
        }

        public Task<Account> CreateAsync(Account account, string Pin, string ConfirmPin)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetByAccountNumberAsync(string AccountNumber)
        {
            throw new NotImplementedException();
        }

        public Account GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Account account, string Pin = null)
        {
            throw new NotImplementedException();
        }
    }
}
