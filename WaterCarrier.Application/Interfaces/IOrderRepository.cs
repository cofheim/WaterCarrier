using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCarrier.Domain.Models;

namespace WaterCarrier.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task<List<Order>> GetAllAsync();
        Task AddAsync(Order entity);
        Task UpdateAsync(Order entity);
        Task DeleteAsync(Guid id);
    }
} 