using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace TeamTasks.Micro.Identity.Models.Identity;

internal static class Configuration
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("TeamTasks.Micro.QuestionsAPI", "Question API"),
            new("TeamTasks.Micro.ImageAPI", "Image API")
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("TeamTasks.Micro.QuestionsAPI", "Question API", new []
                { JwtClaimTypes.Name})
            {
                Scopes = {"TeamTasks.Micro.QuestionsAPI"}
            },
            new("TeamTasks.Micro.ImageAPI", "Image API", new []
                { JwtClaimTypes.Name})
            {
                Scopes = {"TeamTasks.Micro.ImageAPI"}
            },
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "TeamTasks.Micro-image",
                ClientName = "TeamTasks.Micro Image",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris =
                {
                    "http://localhost:44460/signin"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:44460"
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:44460/signout"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "TeamTasks.Micro.ImageAPI"
                },
                AllowAccessTokensViaBrowser = true
            },
            new Client
            {
                ClientId = "TeamTasks.Micro-question",
                ClientName = "TeamTasks.Micro Question",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris =
                {
                    "http://localhost:44460/signin"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:44460"
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:44460/signout"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "TeamTasks.Micro.QuestionAPI"
                },
                AllowAccessTokensViaBrowser = true
            }
        };
}