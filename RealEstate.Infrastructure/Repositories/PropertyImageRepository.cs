using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

public class PropertyImageRepository : IPropertyImageRepository
{
    private readonly IMongoCollection<PropertyImage> _collection;

    public PropertyImageRepository(MongoDbContext context)
    {
        _collection = context.PropertyImages;
    }
    public async Task<IEnumerable<PropertyImage>> GetAllAsync()
    {
        return await _collection.Find(Builders<PropertyImage>.Filter.Empty).ToListAsync();
    }

    public async Task<PropertyImage?> GetByIdAsync(string id)
    {
        return await _collection.Find(p => p.IdPropertyImage == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(PropertyImage propertyImage)
    {
        await _collection.InsertOneAsync(propertyImage);
    }

}
