using api.Domain;
using Microsoft.AspNetCore.Identity;

namespace api.Infrastructure;

public sealed class AspPasswordHasher : IPasswordHasher
{
    readonly static PasswordHasher<Chef> _passwordHasher = new PasswordHasher<Chef>();

    public string Hash(Chef chef, string password) => _passwordHasher.HashPassword(chef, password);
    public PasswordVerificationResult VerifyHashedPassword(Chef chef, string hashedPassword, string providedPassword) => _passwordHasher.VerifyHashedPassword(chef, hashedPassword, providedPassword);
}