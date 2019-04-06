using System;
using SuperDigital.DigitalAccount.Data.Repositories.DB;
using SuperDigital.DigitalAccount.Domain.Domain;
using SuperDigital.DigitalAccount.Domain.Entities;
using SuperDigital.DigitalAccount.Domain.Repository;

namespace SuperDigital.DigitalAccount.Data.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private DbSQLManager db;

        public TransactionRepository()
        {
            var conn = ModuleConfiguration.ConnectionString;
            db = new DbSQLManager(conn);
        }

        public Transaction GetById(Guid id)
        {
            return db.GetFirsOrDefault<Transaction>($@"SELECT * FROM [Transaction] WHERE Id = @id AND Active = 1", new { id });
        }

        public void Add(Transaction transaction)
        {
            var parameter = new
            {
                transaction.FromCheckingAccountId,
                transaction.ToCheckingAccountId,
                transaction.Amount,
                transaction.Created
            };

            var sqlInsert = $@"
                    INSERT INTO [Transaction]
                            ( FromCheckingAccountId ,
                              ToCheckingAccountId ,
                              Amount ,
                              Active ,
                              Created
                            )
                    VALUES  ( @FromCheckingAccountId ,
                              @ToCheckingAccountId ,
                              @Amount,
                              1 , 
                              @Created
                            )";

            db.ExecuteScalar(sqlInsert, parameter);
        }

        public void Update(Transaction transaction)
        {
            transaction.Updated = DateTime.UtcNow;

            var parameter = new
            {
                transaction.Id,
                transaction.FromCheckingAccountId,
                transaction.ToCheckingAccountId,
                transaction.Amount,
                transaction.Updated
            };

            var sqlUpdate = $@"UPDATE  [Transaction]
                            SET     FromCheckingAccountId = @FromCheckingAccountId,
                                    ToCheckingAccountId = @ToCheckingAccountId,
                                    Amount = @Amount
                            WHERE   Id = @Id
                                    AND Active = 1";

            db.ExecuteScalar(sqlUpdate, parameter);
        }

        public void Remove(Transaction transaction)
        {
            db.ExecuteCommand($@"UPDATE [Transaction] SET Active = 0, Updated = @updated WHERE Id = @id", new { updated = DateTime.UtcNow, id = transaction.Id });
        }
    }
}
