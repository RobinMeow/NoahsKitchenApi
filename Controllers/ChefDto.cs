using System.ComponentModel.DataAnnotations;

namespace api.Controllers;

public sealed class ChefDto : EntityDto
{
    [Required]
    public override int ModelVersion { get; init; } = Domain.Chef.MODEL_VERSION;

    [Required]
    public string Name { get; set; } = null!;

    public string? Email { get; set; }
}
