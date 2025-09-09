using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence;

public static class MongoMapping
{
    private static void RegisterClassMap<T>() where T : class, IEntity
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
        {
            BsonClassMap.RegisterClassMap<T>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                  .SetIdGenerator(StringObjectIdGenerator.Instance)
                  .SetSerializer(
                      new MongoDB.Bson.Serialization.Serializers.StringSerializer(
                          MongoDB.Bson.BsonType.ObjectId
                      )
                  );
            });
        }
    }

    public static void Configure()
    {
        RegisterClassMap<Property>();
        RegisterClassMap<Owner>();
        RegisterClassMap<PropertyImage>();
        RegisterClassMap<PropertyTrace>();
    }
}
