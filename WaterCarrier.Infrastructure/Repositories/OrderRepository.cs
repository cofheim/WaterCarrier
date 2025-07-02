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
    public class OrderRepository : IOrderRepository
    {
        private readonly ISession _session;

        public OrderRepository(ISession session)
        {
            _session = session;
        }

        public async Task AddAsync(Order order)
        {
            var entity = OrderMapper.ToEntity(order);
            await _session.SaveAsync(entity);
            await _session.FlushAsync();
        }

        public async Task<(bool success, string errorMessage)> DeleteAsync(Guid id)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var entity = await _session.GetAsync<OrderEntity>(id);
                    if (entity == null)
                    {
                        return (false, "Заказ не найден");
                    }

                    await _session.DeleteAsync(entity);
                    await transaction.CommitAsync();
                    return (true, string.Empty);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, $"Ошибка при удалении заказа: {ex.Message}");
                }
            }
        }

        public async Task<List<Order>> GetAllAsync()
        {
            var entities = await _session.Query<OrderEntity>().ToListAsync();
            return entities.Select(OrderMapper.ToDomain).ToList();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            var entity = await _session.GetAsync<OrderEntity>(id);
            return entity != null ? OrderMapper.ToDomain(entity) : null;
        }

        public async Task UpdateAsync(Order order)
        {
            var entity = OrderMapper.ToEntity(order);
            await _session.MergeAsync(entity);
            await _session.FlushAsync();
        }
    }
} 