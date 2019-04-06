using SuperDigital.DigitalAccount.Data.Repositories.DB;
using SuperDigital.DigitalAccount.Domain.Domain;
using SuperDigital.DigitalAccount.Domain.Entities;
using SuperDigital.DigitalAccount.Domain.Repository;

namespace SuperDigital.DigitalAccount.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private DbSQLManager db;

        public UserRepository()
        {
            var conn = ModuleConfiguration.ConnectionString;
            db = new DbSQLManager(conn);
        }

        public User Authenticate(string email, string password)
        {
            password = CrossCutting.ExtensionMethods.PasswordExtensions.GenerateSHA1(password);

            string query = $@"SELECT * FROM  [User]
                            WHERE   Email = @email
                                    AND Password = @password
                                    AND Active = 1";

            return db.GetFirsOrDefault<User>(query, new { email, password });
        }
    }
}
