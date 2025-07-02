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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ISession _session;

        public EmployeeRepository(ISession session)
        {
            _session = session;
        }

        public async Task AddAsync(Employee employee)
        {
            var entity = EmployeeMapper.ToEntity(employee);
            await _session.SaveAsync(entity);
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _session.GetAsync<EmployeeEntity>(id);
            if (entity != null)
            {
                await _session.DeleteAsync(entity);
                await _session.FlushAsync();
            }
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            var entities = await _session.Query<EmployeeEntity>().ToListAsync();
            return entities.Select(EmployeeMapper.ToDomain).ToList();
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            var entity = await _session.GetAsync<EmployeeEntity>(id);
            return entity != null ? EmployeeMapper.ToDomain(entity) : null;
        }

        public async Task UpdateAsync(Employee employee)
        {
            var entity = EmployeeMapper.ToEntity(employee);
            await _session.MergeAsync(entity);
            await _session.FlushAsync();
        }
    }
} 