using SuperDigital.DigitalAccount.Domain.Entities;

namespace SuperDigital.DigitalAccount.Domain.Repository
{
    public interface ICheckingAccountRepository
    {
        CheckingAccount GetByAccountNumber(long accountNumber);
        void UpdateBalance(long accountNumber, decimal newBalance);
    }
}
