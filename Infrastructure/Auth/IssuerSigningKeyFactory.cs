using System.Text;
using api.Domain.Auth;
using Microsoft.IdentityModel.Tokens;

namespace api.Infrastructure.Auth;

public sealed class IssuerSigningKeyFactory : IIssuerSigningKeyFactory
{
    private readonly BearerConfig _bearerConfig;

    public IssuerSigningKeyFactory(IConfiguration configuration)
    : this(configuration.GetSection(nameof(BearerConfig)).Get<BearerConfig>()
            ?? throw new ArgumentException($"Couldnt not find {nameof(BearerConfig)} in configuration."))
    {
    }

    public IssuerSigningKeyFactory(BearerConfig bearerConfig)
    {
        _bearerConfig = bearerConfig;
    }

    public SecurityKey Create()
    {
        byte[] bytes = Encoding.UTF8.GetBytes(_bearerConfig.SigningKey);
        return new SymmetricSecurityKey(bytes);
    }
}