namespace TeamTasks.Micro.Identity.Models.Identity;

public class JwtOptions
{
    public string Secret { get; set; } = null!;

    public List<string> ValidAudiences { get; set; } = null!;
    
    public List<string> ValidIssuers { get; set; } = null!;
    public int Expire { get; set; } = 3600;
    public int RefreshTokenExpire { get; set; } = 20160;
}