using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services.Interfaces;
using Data.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Account> AuthenticateAsync(string AccountNumber, string Pin)
        {
            var account = await _context.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefaultAsync();
            if (account == null)
            {
                return null;
            }

            //We need to verify the pinHash
            if (!VerifyPinMash(Pin, account.PinHash, account.PinSalt))
            {
                return null;
            }

            //Authentication is passed
            return account;
        }

        private static bool VerifyPinMash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(Pin))
            {
                throw new ArgumentNullException("Pin");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var coumptedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));
                for (int i = 0; i < coumptedPinHash.Length; i++)
                {
                    if (coumptedPinHash[i] != pinHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public async Task<Account> CreateAsync(Account account, string Pin, string ConfirmPin)
        {
            if (_context.Accounts.Any(x => x.Email == account.Email))
            {
                throw new ApplicationException("An account already exist with this email");
            }

            if (!Pin.Equals(ConfirmPin))
            {
                throw new ArgumentException("Pins do not match", "Pin");
            }
            //We are hashing/encrypting pin 
            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt); //This is a crypto method

            account.PinHash = pinHash;
            account.PinSalt = pinSalt;

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return account;
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }


        public async Task DeleteAsync(int Id)
        {
            var account =  _context.Accounts.Find(Id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            var result = await _context.Accounts.ToListAsync();
            return result;
        }

        public async Task<Account> GetByAccountNumberAsync(string AccountNumber)
        {
            var account = await _context.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefaultAsync();
            if (account == null)
            {
                return null;
            }
            return account;
        }

        public async Task<Account> GetByIdAsync(int Id)
        {
            var account = await _context.Accounts.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (account == null)
            {
                return null;
            }
            return account;
        }

        /// <summary>
        /// We want to allow the user to be able to change to only email, phone number and pin
        /// </summary>
        /// <param name="account"></param>
        /// <param name="Pin"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task UpdateAsync(Account account, string Pin = null)
        {
            var accountToBeUpdated = await _context.Accounts.Where(x => x.Email == account.Email).SingleOrDefaultAsync();
            if (accountToBeUpdated == null)
            {
                throw new ApplicationException("Account does not exist");
            }
            if (!string.IsNullOrWhiteSpace(account.Email))
            {
                if(_context.Accounts.Any(x => x.Email == account.Email))
                {
                    throw new ApplicationException("This email " + account.Email + " already exists");
                }
                accountToBeUpdated.Email  = account.Email;
            }

            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                if (_context.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber))
                {
                    throw new ApplicationException("This email " + account.PhoneNumber + " already exists");
                }
                accountToBeUpdated.PhoneNumber = account.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);

                accountToBeUpdated.PinHash = pinHash;
                accountToBeUpdated.PinSalt = pinSalt;
            }
            accountToBeUpdated.DateLastUpdated = DateTime.Now;

            _context.Accounts.Update(accountToBeUpdated);
            await _context.SaveChangesAsync();
        }    


        //public Task<bool> SaveAsync()
        //{
        //    throw new NotImplementedException();
        //}

        
    }
}
