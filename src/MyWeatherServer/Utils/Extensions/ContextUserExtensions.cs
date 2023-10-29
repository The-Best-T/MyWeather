using System.Security.Claims;

namespace Utils.Extensions;

public static class ContextUserExtensions
{
    public static string GetUserId(
        this ClaimsPrincipal claims)
    {
        return claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
    }

}