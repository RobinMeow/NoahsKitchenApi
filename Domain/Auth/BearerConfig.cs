namespace api.Domain.Auth;

public sealed class BearerConfig
{
    public string SigningKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string[] Audiences { get; set; } = null!;
}
