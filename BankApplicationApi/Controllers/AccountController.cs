using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Services.Interfaces;
using Data.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("RegisterNewAccount")]
        public async Task<IActionResult> RegisterNewAccountAsync([FromBody] RegisterNewAccountModel newAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(newAccount);
            }

            var account = _mapper.Map<Account>(newAccount);
            var result = await _accountService.CreateAsync(account, newAccount.Pin, newAccount.ConfirmPin);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccountsAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            var result = await _accountService.GetAllAccountsAsync();
            var account = _mapper.Map<Account>(result);
            return Ok(account);
        }


        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateModel authenticateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(authenticateModel);
            }
            var result = await _accountService.AuthenticateAsync(authenticateModel.AccountNumber, authenticateModel.Pin);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetByAccountNumber")]
        public async Task<IActionResult> GetByAccountNumberAsync(string AccountNumber)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
            {
                return BadRequest("Account Number must be 10-digit");
            }
            var account = await _accountService.GetByAccountNumberAsync(AccountNumber);
            var result = _mapper.Map<GetAccountModel>(account);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAccountById")]
        public async Task<IActionResult> GetAccountByIdAsync(int Id)
        {
            var account = await _accountService.GetByIdAsync(Id);
            var result = _mapper.Map<GetAccountModel>(account);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<IActionResult> UpdateAccountAsync([FromBody] UpdateAccountModel updateAccountModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(updateAccountModel);
            }
            var account = _mapper.Map<Account>(updateAccountModel);
            await _accountService.UpdateAsync(account, updateAccountModel.Pin);
            return Ok();
        }


    }




}
