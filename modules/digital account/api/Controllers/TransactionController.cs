using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperDigital.DigitalAccount.Application.Contract;
using SuperDigital.DigitalAccount.Application.Models;

namespace SuperDigital.DigitalAccount.SuperDigital.DigitalAccount.Api.Front
{
    [Authorize]
    [Route("v1/transaction")]
    public class TransactionController : BaseController
    {
        private readonly ITransactionApplication _transactionApplication;

        public TransactionController(ITransactionApplication transactionApplication)
        {
            this._transactionApplication = transactionApplication;
        }

        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody]TransferRequest transferRequest)
        {
            return Result(this._transactionApplication.Transfer(this.UserId.ToString(), transferRequest));
        }
    }
}