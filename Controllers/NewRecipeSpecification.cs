using api.Domain;

namespace api.Controllers;

/// <summary>Uses the <see cref="RecipeValidators"> to ensure the data is valid. Adds ErrorMessages to the DTO accordingly. Returns true when it contains invalid data.</summary>
public sealed class NewRecipeSpecification
{
    readonly NewRecipeDto _newRecipe;

    public NewRecipeSpecification(NewRecipeDto newRecipe)
    {
        _newRecipe = newRecipe;
    }

    /// <summary>Validates the fields and returns true if the data is valid.</summary>
    public bool IsSatisfied()
    {
        ValidateName(_newRecipe.Name);

        return !_newRecipe.HasErrors();
    }

    void ValidateName(string? name)
    {
        if (name == null)
            _newRecipe.AddErrorMessage($"Name is required.");
        else if (name.Length < RecipeValidators.NAME_MIN_LENGTH)
            _newRecipe.AddErrorMessage($"Name has to be greater than {RecipeValidators.NAME_MIN_LENGTH - 1}.");
        else if (name.Length > RecipeValidators.NAME_MAX_LENGTH)
            _newRecipe.AddErrorMessage($"Name has to be less than {RecipeValidators.NAME_MAX_LENGTH + 1}.");
    }
}
