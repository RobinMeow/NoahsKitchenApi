using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Domain;
using api.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

    public AuthController(
        DbContext dbContext,
        ILogger<RecipeController> logger,
        IPasswordHasher passwordHasher,
        IConfiguration configuration,
        IIssuerSigningKeyFactory issuerSigningKeyFactory
    )
    {
        _chefRepository = dbContext.ChefRepository;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _bearerConfig = configuration.GetSection(nameof(BearerConfig)).Get<BearerConfig>()!;
        _issuerSigningKeyFactory = issuerSigningKeyFactory;
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

        string token = CreateToken(chef);

        return Ok(token);
    }

    private string CreateToken(Chef chef)
    {
        // ToDo: ITokenGenerator
        List<Claim> claims = new List<Claim>() {
            new Claim(ClaimTypes.Name, chef.Name),
        };

        SecurityKey securityKey = _issuerSigningKeyFactory.Create();

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: signingCredentials,
            issuer: _bearerConfig.Issuer,
            audience: String.Join(",", _bearerConfig.Audiences) // https://www.ibm.com/docs/en/datapower-gateway/2018.4?topic=commands-aud-claim "a comma-separated string of values"
            );

        string jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}
