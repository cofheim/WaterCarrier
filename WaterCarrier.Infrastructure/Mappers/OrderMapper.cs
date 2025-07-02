using WaterCarrier.Domain.Models;
using WaterCarrier.Infrastructure.Persistence.Entities;

namespace WaterCarrier.Infrastructure.Mappers
{
    public static class OrderMapper
    {
        public static Order ToDomain(OrderEntity entity)
        {
            var employee = EmployeeMapper.ToDomain(entity.Employee);
            var counterparty = CounterpartyMapper.ToDomain(entity.Counterparty);
            return new Order(entity.Id, entity.Date, entity.Amount, employee, counterparty);
        }

        public static OrderEntity ToEntity(Order model)
        {
            return new OrderEntity
            {
                Id = model.Id,
                Date = model.Date,
                Amount = model.Amount,
                Employee = EmployeeMapper.ToEntity(model.Employee),
                Counterparty = CounterpartyMapper.ToEntity(model.Counterparty)
            };
        }
    }
} 