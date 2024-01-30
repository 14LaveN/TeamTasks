using System.Text.Json;
using TeamTasks.Micro.Identity.Extensions;
using TeamTasks.Micro.Identity.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TeamTasks.Database.Identity;

namespace TeamTasks.Micro.Identity.Controllers;

[Route("tokens")]
public class TokensController(IOptions<JwtOptions> jwtOptions,
    UserDbContext context) : ApiBaseController
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh(string refreshToken)
    {
        var decryptedData = AesEncryptor.Decrypt(_jwtOptions.Secret, refreshToken);
        var data = JsonSerializer.Deserialize<RefreshTokenData>(decryptedData);

        if (data == null)
        {
            return BadRequest();
        }

        if (data.Expire < DateTime.UtcNow)
        {
            return BadRequest();
        }
        
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == data.UserId);

        if (user == null) return BadRequest();

        var (newRefreshToken, refreshTokenExpireAt) = user.GenerateRefreshToken(_jwtOptions);
        
        return Ok(new LoginResponse<TokensController>
        {
            Description = "fпаврварпвапваfdg",
            Data = null,
            StatusCode = Domain.Enumerations.StatusCode.Ok,
            AccessToken = user.GenerateAccessToken(_jwtOptions),
            RefreshToken = newRefreshToken,
            RefreshTokenExpireAt = refreshTokenExpireAt
        });
    }
}