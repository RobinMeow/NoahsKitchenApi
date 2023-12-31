using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using api.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Auth;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("[controller]")]
public sealed class AuthController(
    DbContext _context,
    ILogger<AuthController> _logger,
    IPasswordHasher _passwordHasher,
    IJwtFactory _jwtFactory
    ) : GkbController
{
    readonly IChefRepository _chefRepository = _context.ChefRepository;
    readonly ILogger<AuthController> _logger = _logger;
    readonly IPasswordHasher _passwordHasher = _passwordHasher;
    readonly IJwtFactory _jwtFactory = _jwtFactory;

    /// <summary>
    /// sign up a new user
    /// </summary>
    /// <param name="newChef">the user to sign up</param>
    /// <returns>201 Created</returns>
    [HttpPost(nameof(RegisterAsync))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> RegisterAsync([Required] RegisterChefDto newChef)
    {
        string chefname = newChef.Name;

        try
        {
            Chef? chefWithSameName = await _chefRepository.GetByNameAsync(chefname);

            if (chefWithSameName != null)
                return Results.BadRequest($"Chefname ist bereits vergeben.");

            if (newChef.Email != null)
            {
                Chef? chefWithSameEmail = await _chefRepository.GetByEmailAsync(newChef.Email);

                if (chefWithSameEmail != null)
                    return Results.BadRequest($"Email ist bereits vergeben.");
            }

            Chef chef = new Chef(
                chefname,
                EntityId.New()
            )
            {
                Email = newChef.Email
            };

            chef.SetPassword(newChef.Password, _passwordHasher);

            await _chefRepository.AddAsync(chef).ConfigureAwait(false);

            return Results.Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, CreateErrorMessage(nameof(AuthController), nameof(RegisterAsync)), newChef);
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost(nameof(LoginAsync))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public async Task<ActionResult<string>> LoginAsync([Required] LoginDto login)
    {
        try
        {
            Chef? chef = await _chefRepository.GetByNameAsync(login.Name);

            if (chef == null)
            {
                return BadRequest("User not found.");
            }

            PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(chef, chef.PasswordHash, login.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid password.");
            }

            string token = _jwtFactory.Create(chef);

            return Ok(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, CreateErrorMessage(nameof(AuthController), nameof(LoginAsync)), login);
            return Status_500_Internal_Server_Error;
        }
    }

    [HttpPost(nameof(DeleteAsync))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([Required] DeleteChefDto deleteChef)
    {
        try
        {
            Chef? chef = await _chefRepository.GetByNameAsync(deleteChef.Name);

            if (chef == null)
            {
                return BadRequest("User not found.");
            }

            PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(chef, chef.PasswordHash, deleteChef.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid password.");
            }

            await _chefRepository.RemoveAsync(deleteChef.Name);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, CreateErrorMessage(nameof(AuthController), nameof(DeleteAsync)), deleteChef);
            return Status_500_Internal_Server_Error;
        }
    }
}
