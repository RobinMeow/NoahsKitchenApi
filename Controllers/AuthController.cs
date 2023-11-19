using api.Domain;
using api.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthController : GkbController
{
    readonly IChefRepository _chefRepository;
    readonly ILogger<RecipeController> _logger;
    readonly IPasswordHasher _passwordHasher;
    readonly BearerConfig _bearerConfig;
    readonly IIssuerSigningKeyFactory _issuerSigningKeyFactory;
    readonly IJwtFactory _jwtFactory;

    public AuthController(
        DbContext dbContext,
        ILogger<RecipeController> logger,
        IPasswordHasher passwordHasher,
        IConfiguration configuration,
        IIssuerSigningKeyFactory issuerSigningKeyFactory,
        IJwtFactory jwtFactory
    )
    {
        _chefRepository = dbContext.ChefRepository;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _bearerConfig = configuration.GetSection(nameof(BearerConfig)).Get<BearerConfig>()!;
        _issuerSigningKeyFactory = issuerSigningKeyFactory;
        _jwtFactory = jwtFactory;
    }

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(NewChef newChef)
    {
        string chefname = newChef.Name;
        try
        {
            if (string.IsNullOrWhiteSpace(chefname))
                return BadRequest(new { notifications = new string[] { $"Chefname darf nicht leer sein." } });

            if (chefname.Length < 3)
                return BadRequest(new { notifications = new string[] { $"Chefname muss mind. 3 Zeichen enthalten." } });

            if (chefname.Length > 20)
                return BadRequest(new { notifications = new string[] { $"Chefname darf nicht mehr als 20 Zeichen enthalten." } });

            // validate username, and check for existing ones.
            // read email and userid from claim
            // write into database
            IEnumerable<Chef> chefs = await _chefRepository.GetAllAsync();

            bool nameIsInUse = await _chefRepository.NameIsAlreadyTakenAsync(chefname);

            if (nameIsInUse)
                return BadRequest(new { notifications = new string[] { $"Chefname ist bereits vergeben." } });

            Chef chef = new Chef(
                chefname,
                EntityId.New()
            );

            chef.SetPassword(newChef.Password, _passwordHasher);

            await _chefRepository.AddAsync(chef);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, CreateErrorMessage(nameof(AuthController), nameof(Register)), chefname);
            return Status_500_Internal_Server_Error;
        }
    }

    [HttpPost(nameof(Login))]
    public async Task<ActionResult<string>> Login(string name, string password)
    {
        Chef chef = await _chefRepository.GetAsync(name);

        if (chef == null || chef.Name != name)
        {
            return BadRequest("User not found");
        }

        PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(chef, chef.PasswordHash, password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return BadRequest("Invalid password.");
        }

        string token = _jwtFactory.Create(chef);

        return Ok(token);
    }
}
