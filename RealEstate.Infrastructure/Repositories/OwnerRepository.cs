using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly IMongoCollection<Owner> _collection;
    public OwnerRepository(MongoDbContext context)
    {
        _collection = context.Owners;
    }

    public async Task AddAsync(Owner owner)
    {
        await _collection.InsertOneAsync(owner);
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Owner>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Owner?> GetByIdAsync(string id)
    {
        return await _collection.Find(p => p.IdOwner == id).FirstOrDefaultAsync();
    }

    public Task UpdateAsync(Owner owner)
    {
        throw new NotImplementedException();
    }

    public async Task<Owner> UpdatePhotoAsync(string id, string photoUrl)
    {
        var filtro = Builders<Owner>.Filter.Eq(o => o.IdOwner, id);
        var update = Builders<Owner>.Update.Set(o => o.Photo, photoUrl);

        var ownerActualizado = await _collection.FindOneAndUpdateAsync(
            filtro,
            update,
            new FindOneAndUpdateOptions<Owner>
            {
                ReturnDocument = ReturnDocument.After
            });

        return ownerActualizado;
    }
}