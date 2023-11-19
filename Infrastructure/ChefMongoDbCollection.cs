using api.Domain;
using MongoDB.Driver;

namespace api.Infrastructure;

public sealed class ChefMongoDbCollection : IChefRepository
{
    readonly IMongoCollection<Chef> _collection;

    public ChefMongoDbCollection(IMongoDatabase database)
    {
        _collection = database.GetCollection<Chef>("chefs");
    }

    public Task AddAsync(Chef chef)
    {
        return _collection.InsertOneAsync(chef);
    }

    public async Task<Chef?> GetByNameAsync(string name)
    {
        IAsyncCursor<Chef> asyncCursor = await _collection.FindAsync((Chef chef) => chef.Name == name);
        return await asyncCursor.FirstOrDefaultAsync();
    }

    public async Task<Chef?> GetByEmailAsync(string email)
    {
        IAsyncCursor<Chef> asyncCursor = await _collection.FindAsync((Chef chef) => chef.Email == email);
        return await asyncCursor.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Chef>> GetAllAsync()
    {
        return await _collection
            .Find<Chef>(_ => true)
            .ToListAsync();
    }

    public async Task<Chef> GetAsync(string name)
    {
        return await _collection
            .Find<Chef>(chef => chef.Name == name)
            .SingleOrDefaultAsync();
    }
}
