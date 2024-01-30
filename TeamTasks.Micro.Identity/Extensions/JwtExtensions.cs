using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TeamTasks.Micro.Identity.Models.Identity;
using Microsoft.IdentityModel.Tokens;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Micro.Identity.Extensions;

public static class JwtExtensions
{
    public static (string, DateTime) GenerateRefreshToken(this User user, JwtOptions jwtOptions)
    {
        var refreshTokenExpireAt = DateTime.UtcNow.AddMinutes(jwtOptions.RefreshTokenExpire);
        var data = new RefreshTokenData
        {
            Expire = refreshTokenExpireAt, 
            UserId = user.Id, 
            Key = StringRandomizer.Randomize()
        };
        
        return (AesEncryptor.Encrypt(jwtOptions.Secret, JsonSerializer.Serialize(data)), refreshTokenExpireAt);
    }
    
    public static string GenerateAccessToken(this User user, JwtOptions jwtOptions)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret.PadRight(64)));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
        if (user.UserName is not null)
        {
            var tokeOptions = new JwtSecurityToken(
                claims: new List<Claim>
                {
                    new (JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new (JwtRegisteredClaimNames.Name, user.UserName),
                    new (JwtRegisteredClaimNames.Email, user.EmailAddress),
                    new (JwtRegisteredClaimNames.GivenName, user.Firstname ?? string.Empty),
                    new (JwtRegisteredClaimNames.FamilyName, user.LastName ?? string.Empty),
                },
                expires: DateTime.Now.AddMinutes(jwtOptions.Expire),
                signingCredentials: signinCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }

        throw new InvalidOperationException();
    }
}