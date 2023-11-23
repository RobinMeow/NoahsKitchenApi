using Microsoft.IdentityModel.Tokens;

namespace api.Domain.Auth;

public interface IIssuerSigningKeyFactory
{
    public SecurityKey Create();
}