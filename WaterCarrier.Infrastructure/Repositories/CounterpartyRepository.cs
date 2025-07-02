using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCarrier.Application.Interfaces;
using WaterCarrier.Domain.Models;
using WaterCarrier.Infrastructure.Mappers;
using WaterCarrier.Infrastructure.Persistence.Entities;

namespace WaterCarrier.Infrastructure.Repositories
{
    public class CounterpartyRepository : ICounterpartyRepository
    {
        private readonly ISession _session;

        public CounterpartyRepository(ISession session)
        {
            _session = session;
        }

        public async Task AddAsync(Counterparty counterparty)
        {
            var entity = CounterpartyMapper.ToEntity(counterparty);
            await _session.SaveAsync(entity);
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _session.GetAsync<CounterpartyEntity>(id);
            if (entity != null)
            {
                await _session.DeleteAsync(entity);
                await _session.FlushAsync();
            }
        }

        public async Task<List<Counterparty>> GetAllAsync()
        {
            var entities = await _session.Query<CounterpartyEntity>().ToListAsync();
            return entities.Select(CounterpartyMapper.ToDomain).ToList();
        }

        public async Task<Counterparty?> GetByIdAsync(Guid id)
        {
            var entity = await _session.GetAsync<CounterpartyEntity>(id);
            return entity != null ? CounterpartyMapper.ToDomain(entity) : null;
        }

        public async Task UpdateAsync(Counterparty counterparty)
        {
            var entity = CounterpartyMapper.ToEntity(counterparty);
            await _session.MergeAsync(entity);
            await _session.FlushAsync();
        }
    }
} 