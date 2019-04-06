using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperDigital.DigitalAccount.Application.Contract;

namespace SuperDigital.DigitalAccount.SuperDigital.DigitalAccount.Api
{
    [Authorize]
    [Route("v1/account")]
    public class AccountController : Controller
    {
        private readonly ICheckingAccountApplication _checkingAccountApplication;

        public AccountController(ICheckingAccountApplication checkingAccountApplication)
        {
            this._checkingAccountApplication = checkingAccountApplication;
        }

        [Route("accountNumber/{accountNumber}")]
        [HttpGet]
        public  IActionResult Get(long accountNumber)
        {
            return Ok(_checkingAccountApplication.GetByAccountNumber(accountNumber));
        }
    }
}