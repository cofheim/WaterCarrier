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

        public async Task<(bool success, string errorMessage)> DeleteAsync(Guid id)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var entity = await _session.GetAsync<EmployeeEntity>(id);
                    if (entity == null)
                    {
                        return (false, "Сотрудник не найден");
                    }

                    // Проверяем, является ли сотрудник куратором контрагентов
                    var counterpartiesCount = await _session.Query<CounterpartyEntity>()
                        .Where(c => c.Curator.Id == id)
                        .CountAsync();

                    if (counterpartiesCount > 0)
                    {
                        return (false, "Невозможно удалить сотрудника, так как он является куратором контрагентов");
                    }

                    // Проверяем, связан ли сотрудник с заказами
                    var ordersCount = await _session.Query<OrderEntity>()
                        .Where(o => o.Employee.Id == id)
                        .CountAsync();

                    if (ordersCount > 0)
                    {
                        return (false, "Невозможно удалить сотрудника, так как он связан с заказами");
                    }

                    await _session.DeleteAsync(entity);
                    await transaction.CommitAsync();
                    return (true, string.Empty);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, $"Ошибка при удалении сотрудника: {ex.Message}");
                }
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