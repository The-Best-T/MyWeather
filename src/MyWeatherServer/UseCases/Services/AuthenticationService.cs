using Core;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UseCases.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Exceptions;
using Utils.Extensions;

namespace UseCases.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthenticationService(
        JwtSettings jwtSettings,
        UserManager<IdentityUser> userManager)
    {
        _jwtSettings = jwtSettings;
        _userManager = userManager;
    }

    public async Task<string> CreateTokenAsync(
        string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail) ??
                   throw new EntityNotFoundException($"User with email: {userEmail} not found");

        var signinCredentials = GetSigninCredentials();
        var claims = user.GetClaims();
        var tokenOptions = GenerateTokenOptions(signinCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private JwtSecurityToken GenerateTokenOptions(
        SigningCredentials signingCredentials,
        IEnumerable<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.ExpireTime)),
            signingCredentials: signingCredentials
        );
        return tokenOptions;
    }

    private SigningCredentials GetSigninCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(GlobalConstants.SecretKeyName)!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
}