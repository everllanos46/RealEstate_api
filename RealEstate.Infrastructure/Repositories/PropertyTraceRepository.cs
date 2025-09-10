using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

public class PropertyTraceRepository : IPropertyTraceRepository
{
    private readonly IMongoCollection<PropertyTrace> _collection;
    public PropertyTraceRepository(MongoDbContext context)
    {
        _collection = context.PropertyTraces;
    }

    public async Task AddAsync(PropertyTrace propertyTrace)
    {
        await _collection.InsertOneAsync(propertyTrace);
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PropertyTrace>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<PropertyTrace?> GetByIdPropertyAsync(string id)
    {
        return await _collection.Find(p => p.IdProperty == id).FirstOrDefaultAsync();
    }

    public Task UpdateAsync(PropertyTrace propertyTrace)
    {
        throw new NotImplementedException();
    }
}