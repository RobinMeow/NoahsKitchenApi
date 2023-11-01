namespace api.Domain;

public interface IPasswordHasher
{
    string Hash(Chef chef, string password);
}

