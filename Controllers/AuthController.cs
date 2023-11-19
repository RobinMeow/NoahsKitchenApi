using api.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthController(
    DbContext _context,
    ILogger<RecipeController> _logger,
    IPasswordHasher _passwordHasher,
    IJwtFactory _jwtFactory
    ) : GkbController
{
    readonly IChefRepository _chefRepository = _context.ChefRepository;
    readonly ILogger<RecipeController> _logger = _logger;
    readonly IPasswordHasher _passwordHasher = _passwordHasher;
    readonly IJwtFactory _jwtFactory = _jwtFactory;

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(NewChef newChef)
    {
        string chefname = newChef.Name;

        if (string.IsNullOrWhiteSpace(chefname))
            return BadRequest(new { notifications = new string[] { $"Chefname darf nicht leer sein." } });

        if (chefname.Length < 3)
            return BadRequest(new { notifications = new string[] { $"Chefname muss mind. 3 Zeichen enthalten." } });

        if (chefname.Length > 20)
            return BadRequest(new { notifications = new string[] { $"Chefname darf nicht mehr als 20 Zeichen enthalten." } });

        try
        {
            // validate username, and check for existing ones.
            // read email and userid from claim
            // write into database
            IEnumerable<Chef> chefs = await _chefRepository.GetAllAsync();

            Chef? chefWithSameName = await _chefRepository.GetByNameAsync(chefname);

            if (chefWithSameName != null)
                return BadRequest(new { notifications = new string[] { $"Chefname ist bereits vergeben." } });

            if (newChef.Email != null)
            {
                Chef? chefWithSameEmail = await _chefRepository.GetByEmailAsync(newChef.Email);

                if (chefWithSameEmail != null)
                    return BadRequest(new { notifications = new string[] { $"Email ist bereits vergeben." } });
            }

            Chef chef = new Chef(
                chefname,
                EntityId.New()
            )
            {
                Email = newChef.Email
            };

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
        Chef? chef = await _chefRepository.GetByNameAsync(name);

        if (chef == null)
        {
            return BadRequest("User not found.");
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
