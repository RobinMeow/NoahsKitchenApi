namespace api.Controllers.Recipes;

public sealed class RecipeDto : EntityDto
{
    public override int ModelVersion { get; init; } = Domain.Recipe.MODEL_VERSION;
    public required string Name { get; set; }
}
