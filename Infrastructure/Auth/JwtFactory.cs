using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Domain;
using api.Domain.Auth;
using Microsoft.IdentityModel.Tokens;

namespace api.Infrastructure.Auth;

public sealed class JwtFactory : IJwtFactory
{
    BearerConfig _bearerConfig;
    IIssuerSigningKeyFactory _issuerSigningKeyFactory;

    public JwtFactory(IConfiguration configuration, IIssuerSigningKeyFactory issuerSigningKeyFactory)
    {
        _issuerSigningKeyFactory = issuerSigningKeyFactory;
        _bearerConfig = configuration.GetSection(nameof(BearerConfig)).Get<BearerConfig>()
            ?? throw new ArgumentException($"Missing dependency injection for {nameof(IConfiguration)}.");
    }

    public string Create(Chef chef)
    {
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
