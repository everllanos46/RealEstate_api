using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

public interface IOwnerRepository
{
    Task<IEnumerable<Owner>> GetAllAsync();
    Task<Owner?> GetByIdAsync(string id);
    Task AddAsync(Owner owner);
    Task UpdateAsync(Owner owner);
    Task DeleteAsync(string id);
}