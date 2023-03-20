using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ICS.User.Application.Utils;

public static class TokenSystemTools
{
    public static string GenerateToken(Domain.Entities.User user)
    {
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("secret.secret.secret.secret.secret.secret.secret.secret");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Login", user.Login),
                    new Claim("Name", user.Name),
                    new Claim("Permissions", JsonConvert.SerializeObject(user.PermissionsIdList)),
                    new Claim(ClaimTypes.Role, ((int) user.Role).ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(10),
            SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static string GenerateSystemToken(string apiName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("secret.secret.secret.secret.secret.secret.secret.secret");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, apiName),
            }),
            Expires = DateTime.UtcNow.AddMinutes(2),
            SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static string GetJWTTokenClaim(string token, string claimName)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
            return claimValue;
        }
        catch (Exception ex)
        {            
            return null;
        }
    }
}
