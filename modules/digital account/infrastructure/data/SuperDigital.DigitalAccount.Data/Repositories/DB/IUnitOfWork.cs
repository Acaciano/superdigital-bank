using System;
using System.Data;

namespace SuperDigital.DigitalAccount.Data.Repositories.DB
{
    public interface IUnitOfWork : IDisposable
    {
        bool _hasConnection { get; set; }
        int TransactionDepth { get; set; }
        IDbTransaction _transaction { get; set; }
        IDbConnection Connection { get; set; }
        IDbCommand CreateCommand();
        void SaveChanges();
        new void Dispose();
    }
}
