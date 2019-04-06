using System;

namespace SuperDigital.DigitalAccount.Domain.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Created = DateTime.UtcNow;
        }
        
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}