using WaterCarrier.Domain.Models;

namespace WaterCarrier.Application.Interfaces
{
    public interface ICounterpartyRepository
    {
        Task<Counterparty> GetByIdAsync(Guid id);
        Task<IEnumerable<Counterparty>> GetAllAsync();
        Task AddAsync(Counterparty entity);
        Task UpdateAsync(Counterparty entity);
        Task DeleteAsync(Guid id);
    }
} 