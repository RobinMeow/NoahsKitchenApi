using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class RecipeController : GkbController
{
    readonly ILogger<RecipeController> _logger;
    readonly IRecipeRepository _recipeRepository;

    public RecipeController(
        ILogger<RecipeController> logger,
        DbContext dbContext
        )
    {
        _logger = logger;
        _recipeRepository = dbContext.RecipeRepository;
    }

    [HttpPost(nameof(Add))]
    public IActionResult Add([FromBody] NewRecipeDto newRecipe)
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
