using System.ComponentModel.DataAnnotations;
using System.Data;

namespace api.Controllers.Auth;

public record class RegisterChefDto
{
    [Required]
    [StringRange(3, 20)]
    public string Name { get; set; } = null!;

    [Required]
    [StringRange(4, 50)]
    public string Password { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; }
}
