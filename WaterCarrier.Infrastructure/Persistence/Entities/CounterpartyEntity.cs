using System;

namespace WaterCarrier.Infrastructure.Persistence.Entities
{
    public class CounterpartyEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Inn { get; set; }
        public virtual EmployeeEntity Curator { get; set; }
    }
} 