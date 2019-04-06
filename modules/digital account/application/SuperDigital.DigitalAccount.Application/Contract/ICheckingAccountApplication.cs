using SuperDigital.DigitalAccount.Domain.Entities;

namespace SuperDigital.DigitalAccount.Application.Contract
{
    public interface ICheckingAccountApplication
    {
        CheckingAccount GetByAccountNumber(long accountNumber);
        CheckingAccount UpdateBalance(long accountNumber, decimal newBalance);
    }
}
