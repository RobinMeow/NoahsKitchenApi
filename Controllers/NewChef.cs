namespace api.Controllers;

public struct NewChef
{
    public NewChef(string chefname, string password)
    {
        Name = chefname;
        Password = password;
    }

    public required string Name { get; init; }

    public required string Password { get; init; }
}
