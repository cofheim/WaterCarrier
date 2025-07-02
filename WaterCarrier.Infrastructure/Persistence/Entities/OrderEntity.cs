using System;

namespace WaterCarrier.Infrastructure.Persistence.Entities
{
    public class OrderEntity
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual EmployeeEntity Employee { get; set; } = null!;
        public virtual CounterpartyEntity Counterparty { get; set; } = null!;
    }
} 