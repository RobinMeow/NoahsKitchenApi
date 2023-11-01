using System.Collections.Generic;
using System.Threading.Tasks;
using api.Domain;
using MongoDB.Driver;

namespace api.Infrastructure;

public sealed class RecipeMongoDbCollection : IRecipeRepository
{
	readonly IMongoCollection<Recipe> _collection;

	public RecipeMongoDbCollection(IMongoDatabase database)
	{
		_collection = database.GetCollection<Recipe>("recipes");
	}

	public async void Add(Recipe recipe)
	{
		await _collection.InsertOneAsync(recipe);
	}

	public async Task<IEnumerable<Recipe>> GetAllAsync()
	{
		return await _collection
	  .Find<Recipe>(_ => true)
	  .ToListAsync();
	}
}
