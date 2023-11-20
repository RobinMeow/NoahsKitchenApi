using System.ComponentModel.DataAnnotations;
using api.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Recipes;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class RecipeController(
    ILogger<RecipeController> logger,
    DbContext _context
    ) : GkbController
{
    readonly ILogger<RecipeController> _logger = logger;
    readonly IRecipeRepository _recipeRepository = _context.RecipeRepository;

    [HttpPost(nameof(Add))]
    public IActionResult Add([Required] NewRecipeDto newRecipe)
    {
        try
        {
            var newRecipeSpecification = new NewRecipeSpecification(newRecipe);
            if (!newRecipeSpecification.IsSatisfied())
                return BadRequest(newRecipe);

            Recipe recipe = Create(newRecipe);
            _recipeRepository.Add(recipe);

            return Ok(recipe.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, CreateErrorMessage(nameof(RecipeController), nameof(Add)), newRecipe);
            return Status_500_Internal_Server_Error;
        }
    }

    static Recipe Create(NewRecipeDto newRecipe)
    {
        System.Diagnostics.Debug.Assert(newRecipe.Name != null);
        return new Recipe(EntityId.New())
        {
            CreatedAt = IsoDateTime.Now,
            Name = newRecipe.Name!
        };
    }

    [HttpGet(nameof(GetAll))]
    public async ValueTask<IActionResult> GetAll()
    {
        try
        {
            IEnumerable<Recipe> recipe = await _recipeRepository.GetAllAsync();
            return Ok(recipe.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, CreateErrorMessage(nameof(RecipeController), nameof(GetAll)));
            return Status_500_Internal_Server_Error;
        }
    }
}
