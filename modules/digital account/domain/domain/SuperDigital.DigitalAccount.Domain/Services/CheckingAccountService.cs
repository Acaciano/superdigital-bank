using SuperDigital.DigitalAccount.Domain.Entities;
using SuperDigital.DigitalAccount.Domain.Repository;
using SuperDigital.DigitalAccount.Domain.Services.Contracts;

namespace SuperDigital.DigitalAccount.Domain.Services
{
    public class CheckingAccountService : ICheckingAccountService
    {
        private readonly ICheckingAccountRepository _checkingAccountRepository;

        public CheckingAccountService(ICheckingAccountRepository checkingAccountRepository)
        {
            _checkingAccountRepository = checkingAccountRepository;
        }

        public CheckingAccount GetByAccountNumber(long number)
        {
            return _checkingAccountRepository.GetByAccountNumber(number);
        }

        public CheckingAccount UpdateBalance(long accountNumber, decimal newBalance)
        {
            CheckingAccount checkingAccount = new CheckingAccount
            {
                Balance = newBalance,
                AccountNumber = accountNumber
            };

            _checkingAccountRepository.UpdateBalance(accountNumber, newBalance);

            return checkingAccount;
        }
    }
}
