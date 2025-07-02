using System;

namespace WaterCarrier.Infrastructure.Persistence.Entities
{
    public class EmployeeEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Patronymic { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual int Position { get; set; } // Enum будет маппиться как int
    }
} 