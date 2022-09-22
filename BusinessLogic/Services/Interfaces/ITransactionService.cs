using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Dtos;
using Domain.Models;

namespace BusinessLogic.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Response> CreateNewTransactionAsync(Transaction transaction);
        Task<Response> FindTransactionByDateAsync(DateTime date);
        Task<Response> MakeDepositAsync(string AccountNumber, decimal Amount, string TransactionPin);
        Task<Response> MakeWithdrawalAsync(string AccountNumber, decimal Amount, string TransactionPin);
        Task<Response> MakeFundsTransferAsync(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);

    }
}
