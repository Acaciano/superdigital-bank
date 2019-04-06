using System;

namespace SuperDigital.DigitalAccount.Domain.Entities
{
    public class CheckingAccount : BaseEntity
    {
        public CheckingAccount()
        {
            Active = true;
            Created = DateTime.UtcNow;
        }

        public long AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }
    }
}
