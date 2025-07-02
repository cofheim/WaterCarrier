using WaterCarrier.Domain.Models;
using WaterCarrier.Infrastructure.Persistence.Entities;
using WaterCarrier.Domain.Enums;

namespace WaterCarrier.Infrastructure.Mappers
{
    public static class EmployeeMapper
    {
        public static Employee ToDomain(EmployeeEntity entity)
        {
            return new Employee(entity.Id, entity.LastName, entity.FirstName, entity.Patronymic, entity.BirthDate, (Position)entity.Position);
        }

        public static EmployeeEntity ToEntity(Employee model)
        {
            return new EmployeeEntity
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                BirthDate = model.BirthDate,
                Position = (int)model.Position
            };
        }
    }
} 