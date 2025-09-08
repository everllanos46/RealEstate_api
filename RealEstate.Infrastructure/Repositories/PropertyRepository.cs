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

    public async Task<(IEnumerable<Property> Properties, long TotalCount)> GetAllAsync(
    string? name,
    string? address,
    decimal? minPrice,
    decimal? maxPrice,
    int pageNumber = 1,
    int pageSize = 10)
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

        var totalCount = await _collection.CountDocumentsAsync(filter);

        var projection = Builders<Property>.Projection
            .Include(p => p.IdProperty)
            .Include(p => p.Name)
            .Include(p => p.Address)
            .Include(p => p.Price)
            .Include(p => p.CodeInternal);

        var properties = await _collection
            .Find(filter)
            .Project<Property>(projection)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (properties, totalCount);
    }



    public async Task<Property?> GetByIdAsync(string id) =>
        await _collection.Find(p => p.IdProperty == id).FirstOrDefaultAsync();

    public async Task AddAsync(Property property) =>
        await _collection.InsertOneAsync(property);

}
