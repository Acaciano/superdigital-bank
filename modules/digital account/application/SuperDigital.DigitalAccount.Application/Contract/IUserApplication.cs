
using SuperDigital.DigitalAccount.Domain.Entities;

namespace SuperDigital.DigitalAccount.Application.Contract
{
    public interface IUserApplication
    {
        User Authenticate(string email, string password);
    }
}
