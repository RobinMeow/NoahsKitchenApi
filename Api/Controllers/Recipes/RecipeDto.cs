using System.ComponentModel.DataAnnotations;

namespace api.Controllers.Recipes;

public sealed class RecipeDto : EntityDto
{
    public override int ModelVersion { get; init; } = Domain.Recipe.MODEL_VERSION;

    [Required]
    public string Name { get; set; } = null!;
}
