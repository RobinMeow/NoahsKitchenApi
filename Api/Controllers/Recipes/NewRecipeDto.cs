using System.ComponentModel.DataAnnotations;

namespace api.Controllers.Recipes;

public sealed class NewRecipeDto : Notification
{
    [Required]
    public string Name { get; set; } = null!;
}
