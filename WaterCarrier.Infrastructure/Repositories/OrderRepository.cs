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
    public class OrderRepository : IOrderRepository
    {
        public async Task AddAsync(Order order)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var orderEntity = OrderMapper.ToEntity(order);
                await session.SaveAsync(orderEntity);
                await transaction.CommitAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var orderEntity = await session.GetAsync<OrderEntity>(id);
                if (orderEntity != null)
                {
                    await session.DeleteAsync(orderEntity);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var orderEntities = await session.Query<OrderEntity>().ToListAsync();
                return orderEntities.Select(OrderMapper.ToDomain);
            }
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var orderEntity = await session.GetAsync<OrderEntity>(id);
                return orderEntity == null ? null : OrderMapper.ToDomain(orderEntity);
            }
        }

        public async Task UpdateAsync(Order order)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var orderEntity = OrderMapper.ToEntity(order);
                await session.UpdateAsync(orderEntity);
                await transaction.CommitAsync();
            }
        }
    }
} 