using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Domain;

public interface IRecipeRepository
{
    void Add(Recipe recipe);
    Task<IEnumerable<Recipe>> GetAllAsync();
}
