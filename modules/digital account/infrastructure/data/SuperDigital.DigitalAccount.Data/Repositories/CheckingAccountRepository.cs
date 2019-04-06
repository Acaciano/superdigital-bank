using SuperDigital.DigitalAccount.Data.Repositories.DB;
using SuperDigital.DigitalAccount.Domain.Domain;
using SuperDigital.DigitalAccount.Domain.Entities;
using SuperDigital.DigitalAccount.Domain.Repository;
using System;

namespace SuperDigital.DigitalAccount.Data.Repository
{
    public class CheckingAccountRepository : ICheckingAccountRepository
    {
        private DbSQLManager db;

        public CheckingAccountRepository()
        {
            var conn = ModuleConfiguration.ConnectionString;
            db = new DbSQLManager(conn);
        }

        public CheckingAccount GetByAccountNumber(long accountNumber)
        {
            return db.GetFirsOrDefault<CheckingAccount>($@"SELECT * FROM [CheckingAccount] WHERE AccountNumber = @accountNumber AND Active = 1", new { accountNumber });
        }

        public void UpdateBalance(long accountNumber, decimal newBalance)
        {
            var parameter = new
            {
                accountNumber,
                newBalance,
                updated = DateTime.UtcNow
            };

            var sqlUpdate = $@"UPDATE  CheckingAccount
                            SET     Balance = @newBalance,
                                    Updated = @updated
                            WHERE   AccountNumber = @accountNumber
                                    AND Active = 1";
            db.ExecuteScalar(sqlUpdate, parameter);
        }
    }
}
