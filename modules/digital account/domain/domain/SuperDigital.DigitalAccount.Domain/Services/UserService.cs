using SuperDigital.DigitalAccount.Domain.Entities;
using SuperDigital.DigitalAccount.Domain.Repository;
using SuperDigital.DigitalAccount.Domain.Services.Contracts;

namespace SuperDigital.DigitalAccount.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Authenticate(string email, string password)
        {
            return _userRepository.Authenticate(email, password);
        }
    }
}
