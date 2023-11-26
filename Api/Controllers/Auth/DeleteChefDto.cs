using System.ComponentModel.DataAnnotations;

namespace api.Controllers.Auth;

public record class DeleteChefDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
