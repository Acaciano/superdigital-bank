using SuperDigital.DigitalAccount.Application.Contract;
using SuperDigital.DigitalAccount.Domain.Entities;
using SuperDigital.DigitalAccount.Domain.Services.Contracts;

namespace SuperDigital.DigitalAccount.Application.Application
{
    public class CheckingAccountApplication : ICheckingAccountApplication
    {
        private readonly ICheckingAccountService _checkingAccountService;

        public CheckingAccountApplication(ICheckingAccountService checkingAccountService)
        {
            _checkingAccountService = checkingAccountService;
        }

        public CheckingAccount GetByAccountNumber(long accountNumber)
        {
            return _checkingAccountService.GetByAccountNumber(accountNumber);
        }

        public CheckingAccount UpdateBalance(long accountNumber, decimal newBalance)
        {
            return _checkingAccountService.UpdateBalance(accountNumber, newBalance);
        }
    }
}
