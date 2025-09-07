using MongoDB.Driver;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string dbName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(dbName);
    }

    public IMongoCollection<Property> Properties => _database.GetCollection<Property>("properties");
    public IMongoCollection<Owner> Owners => _database.GetCollection<Owner>("owners");
    public IMongoCollection<PropertyImage> PropertyImages => _database.GetCollection<PropertyImage>("propertyImages");
    public IMongoCollection<PropertyTrace> PropertyTraces => _database.GetCollection<PropertyTrace>("propertyTraces");
}
