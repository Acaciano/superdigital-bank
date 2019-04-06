using System;
using SuperDigital.DigitalAccount.CrossCutting;
using SuperDigital.DigitalAccount.Domain.Entities;
using SuperDigital.DigitalAccount.Domain.Repository;
using SuperDigital.DigitalAccount.Domain.Services.Contracts;

namespace SuperDigital.DigitalAccount.Domain.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICheckingAccountService _checkingAccountService;

        public TransactionService(ITransactionRepository transactionRepository, ICheckingAccountService checkingAccountService)
        {
            _transactionRepository = transactionRepository;
            _checkingAccountService = checkingAccountService;
        }

        public Transaction GetById(Guid id)
        {
            return _transactionRepository.GetById(id);
        }

        public void Add(Transaction transaction)
        {
            _transactionRepository.Add(transaction);
        }

        public void Update(Transaction transaction)
        {
            _transactionRepository.Update(transaction);
        }

        public void Remove(Transaction transaction)
        {
            _transactionRepository.Remove(transaction);
        }
    }
}
