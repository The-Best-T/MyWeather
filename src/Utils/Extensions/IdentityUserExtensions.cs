using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Utils.Extensions;

public static class IdentityUserExtensions
{
    public static List<Claim> GetClaims(
        this IdentityUser user)
    {
        return new()
        {
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id),
        };
    }
}