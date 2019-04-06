using System;
using System.Data;

namespace SuperDigital.DigitalAccount.Data.Repositories.DB
{
    public class UnitOfWork : IUnitOfWork
    {
        public bool _hasConnection { get; set; }

        public IDbTransaction _transaction { get; set; }

        public IDbConnection Connection { get; set; }
        public int TransactionDepth { get; set; }

        public UnitOfWork(IDbConnection connection, bool hasConnection)
        {
            Connection = connection;
            _hasConnection = hasConnection;
            _transaction = connection.BeginTransaction();
        }

        public IDbCommand CreateCommand()
        {
            var command = Connection.CreateCommand();
            command.Transaction = _transaction;
            return command;
        }

        public void SaveChanges()
        {
            if (TransactionDepth-- != 0)
                return;

            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction already been committed");
            }

            _transaction.Commit();
            _transaction = null;
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }

            if (Connection != null && _hasConnection)
            {
                Connection.Close();
                Connection = null;
            }
        }
    }
}
