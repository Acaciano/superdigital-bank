using SuperDigital.DigitalAccount.Domain.Entities;
using System;

namespace SuperDigital.DigitalAccount.Domain.Repository
{
    public interface ITransactionRepository
    {
        Transaction GetById(Guid id);
        void Add(Transaction transaction);
        void Update(Transaction transaction);
        void Remove(Transaction transaction);
    }
}
