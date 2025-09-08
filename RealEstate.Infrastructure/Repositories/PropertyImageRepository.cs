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
    public async Task<IEnumerable<PropertyImage>> GetAllAsync(IEnumerable<string> propertyIds)
    {
        if (propertyIds == null || !propertyIds.Any())
            return new List<PropertyImage>();

        var filter = Builders<PropertyImage>.Filter.In(p => p.IdProperty, propertyIds)
                     & Builders<PropertyImage>.Filter.Eq(p => p.Enabled, true);

        var totalCount = await _collection.CountDocumentsAsync(filter);

        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<PropertyImage?> GetByIdAsync(string id)
    {
        return await _collection.Find(p => p.IdProperty == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(PropertyImage propertyImage)
    {
        await _collection.InsertOneAsync(propertyImage);
    }

}
