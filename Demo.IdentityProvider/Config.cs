using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Shared;
using System.Collections.Generic;
using System.Security.Claims;

namespace Demo.IdentityProvider
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Username = "Frank",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim(ClaimDeclaration.GivenName, "Frank"),
                        new Claim(ClaimDeclaration.FamilyName, "Underwood"),
                        new Claim(ClaimDeclaration.Role, RoleType.Admin),
                        new Claim(ClaimDeclaration.Role, RoleType.User),
                        new Claim(ClaimDeclaration.Subscriptionlevel, SubscriptionType.Premium),
                    }
                },
                new TestUser
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "Claire",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim(ClaimDeclaration.GivenName, "Claire"),
                        new Claim(ClaimDeclaration.FamilyName, "Underwood"),
                        new Claim(ClaimDeclaration.Role, RoleType.User),
                        new Claim(ClaimDeclaration.Subscriptionlevel, SubscriptionType.Basic),

                    }
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                // Role and subscriptionlevel claims are passed to API in Access Token
                new ApiResource("demoapi","Demo API", new List<string>{ ClaimDeclaration.Role, ClaimDeclaration.Subscriptionlevel })
            };
        }

        // Resources attached to user identity
        // OpenId is required
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(ClaimDeclaration.Roles,"Your roles", new List<string>{ClaimDeclaration.Role}),
                new IdentityResource(ClaimDeclaration.Subscriptionlevel,"Your subscription level", new List<string>{ClaimDeclaration.Subscriptionlevel})
            };
        }


        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName ="Client.SPA",
                    ClientId = "SPA_APP",
                    ClientSecrets = { new Secret("password".Sha256()) },
                    PostLogoutRedirectUris = {$"{Parties.WebClientUrl}/signout-callback-oidc" },
                    RedirectUris = {$"{Parties.WebClientUrl}/signin-oidc","" }, // signin-oidc is default route
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        ClaimDeclaration.Roles,
                        "demoapi",
                        ClaimDeclaration.Subscriptionlevel
                    },



                    /// <summary>
                    ///  TOKEN LIFETIMES
                    /// </summary>
                    // Time that identity token is valid for authentication (default 5min)
                    IdentityTokenLifetime = 300,
                    // Time that access token is valid for authorization (default 1h)
                    // Long lived access is gained with Refresh Tokens
                    AccessTokenLifetime = 60,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    // If RefreshTokenExpiration is set to Sliding, this value is used to bump up lifetime of refresh token
                    SlidingRefreshTokenLifetime = 1296000, 
                    // Prevents outdated claims when access token is updated (by default claims dont change on access token update)
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true, // Must be set to true


                    AllowedGrantTypes = GrantTypes.Hybrid
                }
            };
        }
    }
}
