using System.ComponentModel.DataAnnotations;

namespace api.Controllers.Auth;

public record class RegisterChefDto
{
    [Required]
    [StringRange(3, 20)]
    public string Name { get; set; } = null!;

    [Required]
    [StringRange(4, 50)]
    public string Password { get; set; } = null!;

    [StringRange(5, 254)]
    public string? Email { get; set; }
}
