using SuperDigital.DigitalAccount.Application.Contract;
using SuperDigital.DigitalAccount.Domain.Entities;
using SuperDigital.DigitalAccount.Domain.Services.Contracts;

namespace SuperDigital.DigitalAccount.Application.Application
{
    public class UserApplication : IUserApplication
    {
        private readonly IUserService _userService;

        public UserApplication(IUserService userService)
        {
            _userService = userService;
        }

        public User Authenticate(string email, string password)
        {
            return _userService.Authenticate(email, password);
        }
    }
}
