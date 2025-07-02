using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCarrier.Application.Interfaces;
using WaterCarrier.Domain.Models;
using WaterCarrier.Infrastructure.Mappers;
using WaterCarrier.Infrastructure.Persistence;
using WaterCarrier.Infrastructure.Persistence.Entities;

namespace WaterCarrier.Infrastructure.Repositories
{
    public class CounterpartyRepository : ICounterpartyRepository
    {
        public async Task AddAsync(Counterparty counterparty)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var counterpartyEntity = CounterpartyMapper.ToEntity(counterparty);
                await session.SaveAsync(counterpartyEntity);
                await transaction.CommitAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var counterpartyEntity = await session.GetAsync<CounterpartyEntity>(id);
                if (counterpartyEntity != null)
                {
                    await session.DeleteAsync(counterpartyEntity);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<IEnumerable<Counterparty>> GetAllAsync()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var counterpartyEntities = await session.Query<CounterpartyEntity>().ToListAsync();
                return counterpartyEntities.Select(CounterpartyMapper.ToDomain);
            }
        }

        public async Task<Counterparty> GetByIdAsync(Guid id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var counterpartyEntity = await session.GetAsync<CounterpartyEntity>(id);
                return counterpartyEntity == null ? null : CounterpartyMapper.ToDomain(counterpartyEntity);
            }
        }

        public async Task UpdateAsync(Counterparty counterparty)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var counterpartyEntity = CounterpartyMapper.ToEntity(counterparty);
                await session.UpdateAsync(counterpartyEntity);
                await transaction.CommitAsync();
            }
        }
    }
} 