using System.Security.Claims;
using System.Text;
using InforceTestTask.Application.Options;
using InforceTestTask.Domain.Entities;
using InforceTestTask.Domain.Interfaces;
using InforceTestTask.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace InforceTestTask.Application.Services;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    public string GenerateToken(User user)
    {
        var claims = SetClaims(user);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var handler = new JsonWebTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = signingCredentials,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(options.Value.ExpiredHours)
        };

        return handler.CreateToken(tokenDescriptor);
    }

    private static List<Claim> SetClaims(User user)
    {
        var roleName = Role.GetValues().FirstOrDefault(r => r.Id == user.RoleId)?.Name;
        var claims = new List<Claim>
        {
            new(CustomClaims.UserId, user.Id.ToString()),
            new(CustomClaims.NickName, user.NickName),
            new(CustomClaims.Role, roleName ?? "Unknown"),
        };

        return claims;
    }
}