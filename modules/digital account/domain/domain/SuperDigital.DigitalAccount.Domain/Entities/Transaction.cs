using System;

namespace SuperDigital.DigitalAccount.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid FromCheckingAccountId { get; set; }
        public Guid ToCheckingAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
