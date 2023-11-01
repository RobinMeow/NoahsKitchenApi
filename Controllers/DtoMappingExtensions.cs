using System.Collections.Generic;
using System.Threading.Tasks;
using api.Domain;

namespace api.Controllers;

public static class DtoMappingExtensions
{
    public static RecipeDto ToDto(this Recipe recipe)
    {
        return new RecipeDto
        {
            Id = recipe.Id,
            ModelVersion = recipe.ModelVersion,
            Name = recipe.Name,
            CreatedAt = recipe.CreatedAt
        };
    }

    public static IEnumerable<RecipeDto> ToDto(this IEnumerable<Recipe> recipes)
    {
        ICollection<RecipeDto> list = new List<RecipeDto>();

        Parallel.ForEach(recipes, (recipe) =>
        {
            list.Add(recipe.ToDto());
        });

        return list;
    }
}
