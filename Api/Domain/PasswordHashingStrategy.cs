using Microsoft.AspNetCore.Identity;

namespace api.Domain;

public interface IPasswordHasher
{
    string Hash(Chef chef, string password);
    PasswordVerificationResult VerifyHashedPassword(Chef chef, string hashedPassword, string providedPassword);
}

