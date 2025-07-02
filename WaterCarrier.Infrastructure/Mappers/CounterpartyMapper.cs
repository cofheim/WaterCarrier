using WaterCarrier.Domain.Models;
using WaterCarrier.Infrastructure.Persistence.Entities;

namespace WaterCarrier.Infrastructure.Mappers
{
    public static class CounterpartyMapper
    {
        public static Counterparty ToDomain(CounterpartyEntity entity)
        {
            var curator = EmployeeMapper.ToDomain(entity.Curator);
            return new Counterparty(entity.Id, entity.Name, entity.Inn, curator);
        }

        public static CounterpartyEntity ToEntity(Counterparty model)
        {
            return new CounterpartyEntity
            {
                Id = model.Id,
                Name = model.Name,
                Inn = model.Inn,
                Curator = EmployeeMapper.ToEntity(model.Curator)
            };
        }
    }
} 