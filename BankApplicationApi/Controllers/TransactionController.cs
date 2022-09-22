using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Services.Implementations;
using BusinessLogic.Services.Interfaces;
using Data.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;
        IMapper _mapper;
        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }
        
        [HttpPost]
        [Route("CreateNewTransaction")]
        public async Task<IActionResult> CreateNewTransactionAsync([FromBody] TransactionRequestDto transactionRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(transactionRequest);
            }

            var transaction = _mapper.Map<Transaction>(transactionRequest);
            var result = await _transactionService.CreateNewTransactionAsync(transaction);
            return Ok(result);
        }

        [HttpPost]
        [Route("MakeDeposit")]
        public async Task<IActionResult> MakeDepositAsync(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
            {
                return BadRequest("Account Number must be 10-digit");
            }

            //var transaction = _mapper.Map<Transaction>(transactionRequest);
            var result = await _transactionService.MakeDepositAsync(AccountNumber, Amount, TransactionPin);
            return Ok(result);
        }

        // POST api/<TransactionController>
        [HttpPost]
        [Route("MakeWithdrawal")]
        public async Task<IActionResult> MakeWithdrawalAsync(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
            {
                return BadRequest("Account Number must be 10-digit");
            }

            //var transaction = _mapper.Map<Transaction>(transactionRequest);
            var result = await _transactionService.MakeWithdrawalAsync(AccountNumber, Amount, TransactionPin);
            return Ok(result);
        }

        [HttpPost]
        [Route("MakeFundsTransfer")]
        public async Task<IActionResult> MakeFundsTransferAsync(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(FromAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$") || !Regex.IsMatch(ToAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
            {
                return BadRequest("Account Number must be 10-digit");
            }

            //var transaction = _mapper.Map<Transaction>(transactionRequest);
            var result = await _transactionService.MakeFundsTransferAsync(FromAccount, ToAccount, Amount, TransactionPin);
            return Ok(result);
        }

        // DELETE api/<TransactionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
