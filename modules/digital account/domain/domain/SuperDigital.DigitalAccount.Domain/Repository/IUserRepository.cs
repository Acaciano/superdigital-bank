using SuperDigital.DigitalAccount.Domain.Entities;

namespace SuperDigital.DigitalAccount.Domain.Repository
{
    public interface IUserRepository
    {
        User Authenticate(string email, string password);
    }
}
