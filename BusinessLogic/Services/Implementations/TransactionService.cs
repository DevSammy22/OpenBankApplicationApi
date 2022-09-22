using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BusinessLogic.Services.Interfaces;
using Data.Context;
using Data.Dtos;
using Domain.Commons;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BusinessLogic.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private AppDbContext _context;
        ILogger<TransactionService> _logger;
        private AppSettings _settings;
        private static string _ourBankSettlementAccount;
        private readonly IAccountService _accountService;

        public TransactionService(AppDbContext context, ILogger<TransactionService> logger, IOptions<AppSettings> settings, IAccountService accountService)
        {
            _context = context;
            _logger = logger;
            _settings = settings.Value;
            _ourBankSettlementAccount = _settings.OurBankSettlementAccount;
            _accountService = accountService;
        }

        public async Task<Response> CreateNewTransactionAsync(Transaction transaction)
        {
            Response response = new Response();
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully";
            response.Data = null;
            return response;
        }

        public async Task<Response> FindTransactionByDateAsync(DateTime date)
        {
            Response response = new Response();
            var transaction = await _context.Transactions.Where(x => x.TransactionDate == date).ToListAsync();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction found successfully";
            response.Data = transaction;
            return response;
        }

        public async Task<Response> MakeDepositAsync(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount; //our Bank Settlement Account
            Account destinationAccount; //individual
            Transaction transaction = new Transaction();
            var userAccount = await _accountService.AuthenticateAsync(AccountNumber, TransactionPin);
            if (userAccount == null)
            {
                throw new ApplicationException("Invalid credentials");
            }

            try
            {
                sourceAccount = await _accountService.GetByAccountNumberAsync(_ourBankSettlementAccount);
                destinationAccount = await _accountService.GetByAccountNumberAsync(AccountNumber);
               
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if (_context.Entry(sourceAccount).State == EntityState.Modified &&
                    _context.Entry(destinationAccount).State == EntityState.Modified)
                {
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURED... => {ex.Message}");
            }

            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = TranType.Deposit;
            transaction.TransactionAmount = Amount;
            transaction.TransactionSourceAccount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT =>" +
                $" {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON DATE =>" +
                $" {transaction.TransactionDate} FOR AMOUNT =>{JsonConvert.SerializeObject(transaction.TransactionAmount)} TRANSACTION TYPE =>" +
                $" {JsonConvert.SerializeObject(transaction.TransactionType)} TRANSACTION STATUS =>" +
                $" {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<Response> MakeFundsTransferAsync(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();
            var userAccount = await _accountService.AuthenticateAsync(FromAccount, TransactionPin);
            if (userAccount == null)
            {
                throw new ApplicationException("Invalid credentials");
            }

            try
            {
                //bankSettlementAccount is the destination getting money from the user's account
                sourceAccount = await _accountService.GetByAccountNumberAsync(FromAccount);
                destinationAccount = await _accountService.GetByAccountNumberAsync(ToAccount);
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if (_context.Entry(sourceAccount).State == EntityState.Modified &&
                    _context.Entry(destinationAccount).State == EntityState.Modified)
                {
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURED... => {ex.Message}");
            }

            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = TranType.Transfer;
            transaction.TransactionAmount = Amount;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT =>" +
                $" {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON DATE =>" +
                $" {transaction.TransactionDate} FOR AMOUNT =>{JsonConvert.SerializeObject(transaction.TransactionAmount)} TRANSACTION TYPE =>" +
                $" {JsonConvert.SerializeObject(transaction.TransactionType)} TRANSACTION STATUS =>" +
                $" {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<Response> MakeWithdrawalAsync(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount; 
            Account destinationAccount;
            Transaction transaction = new Transaction();
            var userAccount = await _accountService.AuthenticateAsync(AccountNumber, TransactionPin);
            if (userAccount == null)
            {
                throw new ApplicationException("Invalid credentials");
            }

            try
            {
                //bankSettlementAccount is the destination getting money from the user's account
                sourceAccount = await _accountService.GetByAccountNumberAsync(AccountNumber);
                destinationAccount = await _accountService.GetByAccountNumberAsync(_ourBankSettlementAccount);
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if (_context.Entry(sourceAccount).State == EntityState.Modified &&
                    _context.Entry(destinationAccount).State == EntityState.Modified)
                {
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successful";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURED... => {ex.Message}");
            }

            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = TranType.Withdrawal;
            transaction.TransactionAmount = Amount;
            transaction.TransactionSourceAccount = AccountNumber;
            transaction.TransactionDestinationAccount = _ourBankSettlementAccount;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT =>" +
                $" {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON DATE =>" +
                $" {transaction.TransactionDate} FOR AMOUNT =>{JsonConvert.SerializeObject(transaction.TransactionAmount)} TRANSACTION TYPE =>" +
                $" {JsonConvert.SerializeObject(transaction.TransactionType)} TRANSACTION STATUS =>" +
                $" {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return response;
        }


    }
}
