using SuperDigital.DigitalAccount.Domain.Entities;

namespace SuperDigital.DigitalAccount.Domain.Services.Contracts
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
    }
}
