using Microsoft.Extensions.DependencyInjection;
using SuperDigital.DigitalAccount.Application.Application;
using SuperDigital.DigitalAccount.Application.Contract;
using SuperDigital.DigitalAccount.Data.Repository;
using SuperDigital.DigitalAccount.Domain.Repository;
using SuperDigital.DigitalAccount.Domain.Services;
using SuperDigital.DigitalAccount.Domain.Services.Contracts;

namespace SuperDigital.DigitalAccount.Ioc
{
    public class Injector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Application
            services.AddTransient<IUserApplication, UserApplication>();
            services.AddTransient<ICheckingAccountApplication, CheckingAccountApplication>();
            services.AddTransient<ITransactionApplication, TransactionApplication>();

            //Service
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICheckingAccountService, CheckingAccountService>();
            services.AddTransient<ITransactionService, TransactionService>();

            //Repository
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICheckingAccountRepository, CheckingAccountRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
        }
    }
}
