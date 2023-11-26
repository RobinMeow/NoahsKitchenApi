using api.Controllers.Auth;
using api.Domain;
using api.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Auth;

public sealed class ControllerTests
{
    readonly AuthController _authController;

    public ControllerTests()
    {
        DbContext dbContext = Substitute.For<DbContext>();

        _authController = new AuthController(
            dbContext,
            Substitute.For<ILogger<AuthController>>(),
            Substitute.For<IPasswordHasher>(),
            Substitute.For<IJwtFactory>()
        );
    }

    [Fact]
    public async Task Register()
    {
        var requestDto = new RegisterChefDto()
        {
            Name = "Robin",
            Password = "Password"
        };

        var expectedCreatedResult = new CreatedResult();

        CreatedResult? meow = await _authController.RegisterAsync(requestDto) as CreatedResult;

        Equal(expectedCreatedResult.StatusCode, meow?.StatusCode);
    }

    [Fact]
    public async Task Register_With_NUll_Throws()
    {
        await ThrowsAnyAsync<NullReferenceException>(async () => await _authController.RegisterAsync(null!));
    }
}
