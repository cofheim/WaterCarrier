using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCarrier.Domain.Models;

namespace WaterCarrier.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(Guid id);
        Task<List<Employee>> GetAllAsync();
        Task AddAsync(Employee entity);
        Task UpdateAsync(Employee entity);
        Task DeleteAsync(Guid id);
    }
} 