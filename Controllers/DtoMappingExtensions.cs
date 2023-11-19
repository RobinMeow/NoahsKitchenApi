using System.Collections.Concurrent;
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
        ConcurrentQueue<RecipeDto> list = new ConcurrentQueue<RecipeDto>();

        Parallel.ForEach(recipes, (recipe) =>
        {
            list.Enqueue(recipe.ToDto());
        });

        return list;
    }
}
