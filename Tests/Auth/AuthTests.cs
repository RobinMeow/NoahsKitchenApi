using api.Domain;

namespace Auth;

public sealed class ControllerTests
{
    [Fact]
    public void Register()
    {
        var recipe = new Recipe(EntityId.New())
        {
            Name = "Meow",
        };

        Same("Meow", recipe.Name);
    }
}
