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
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task AddAsync(Employee employee)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var employeeEntity = EmployeeMapper.ToEntity(employee);
                await session.SaveAsync(employeeEntity);
                await transaction.CommitAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var employeeEntity = await session.GetAsync<EmployeeEntity>(id);
                if (employeeEntity != null)
                {
                    await session.DeleteAsync(employeeEntity);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var employeeEntities = await session.Query<EmployeeEntity>().ToListAsync();
                return employeeEntities.Select(EmployeeMapper.ToDomain);
            }
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var employeeEntity = await session.GetAsync<EmployeeEntity>(id);
                return employeeEntity == null ? null : EmployeeMapper.ToDomain(employeeEntity);
            }
        }

        public async Task UpdateAsync(Employee employee)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var employeeEntity = EmployeeMapper.ToEntity(employee);
                await session.UpdateAsync(employeeEntity);
                await transaction.CommitAsync();
            }
        }
    }
} 