namespace Core;

public class JwtSettings
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string ExpireTime { get; init; }
}