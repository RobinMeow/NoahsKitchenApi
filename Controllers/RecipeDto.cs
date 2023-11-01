namespace api.Controllers;

public sealed class RecipeDto : EntityDto
{
    public override int ModelVersion { get; init; } = Domain.Recipe.MODEL_VERSION;
    public required string Name { get; set; }
}

public sealed class ChefDto : EntityDto
{
    public override int ModelVersion { get; init; } = Domain.Chef.MODEL_VERSION;
    public required string Name { get; set; }
    public required string Email { get; set; }
}
