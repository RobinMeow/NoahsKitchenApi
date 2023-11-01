namespace api.Domain;

public abstract class DbContext
{
    public abstract IRecipeRepository RecipeRepository { get; init; }

    public abstract IChefRepository ChefRepository { get; init; }
}
