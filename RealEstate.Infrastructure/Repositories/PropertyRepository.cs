using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _collection;

    public PropertyRepository(MongoDbContext context)
    {
        _collection = context.Properties;
    }

    public async Task<IEnumerable<Property>> GetAllAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice)
    {
        var filter = Builders<Property>.Filter.Empty;

        if (!string.IsNullOrEmpty(name))
            filter &= Builders<Property>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

        if (!string.IsNullOrEmpty(address))
            filter &= Builders<Property>.Filter.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(address, "i"));

        if (minPrice.HasValue)
            filter &= Builders<Property>.Filter.Gte(p => p.Price, minPrice.Value);

        if (maxPrice.HasValue)
            filter &= Builders<Property>.Filter.Lte(p => p.Price, maxPrice.Value);

        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(string id) =>
        await _collection.Find(p => p.IdOwner == id).FirstOrDefaultAsync();

    public async Task AddAsync(Property property) =>
        await _collection.InsertOneAsync(property);

}
