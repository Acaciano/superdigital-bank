using SuperDigital.DigitalAccount.Domain.Entities;

namespace SuperDigital.DigitalAccount.Domain.Services.Contracts
{
    public interface ICheckingAccountService
    {
        CheckingAccount GetByAccountNumber(long accountNumber);
        CheckingAccount UpdateBalance(long accountNumber, decimal newBalance);
    }
}
