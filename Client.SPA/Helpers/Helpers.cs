using Client.SPA.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Shared;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.SPA.Helpers
{
    public static class Helpers
    {
        public static async Task<UserDto> GetUserInfoAsync(ClaimsPrincipal User, HttpContext context)
        {
            // Access token that is used for API authorization
            // Needs to be set as Bearer token for each Ajax request
            var access_token = await context.GetTokenAsync("access_token");
            // Each ajax request needs to check if token is soon to be expired
            // so renew token method can be called in time
            var expires_at = await context.GetTokenAsync("expires_at");

            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            var user = new UserDto
            {
                Id = userId,
                Token = access_token,
                TokenRenewalTime = CalculateRefreshTokenRenewalTime(expires_at),
                TokenExpirationTime = (DateTime.Parse(expires_at).ToUniversalTime().Ticks),
                GivenName = User.Claims.FirstOrDefault(c => c.Type == ClaimDeclaration.GivenName).Value,
                FamilyName = User.Claims.FirstOrDefault(c => c.Type == ClaimDeclaration.FamilyName).Value,
                Roles = User.Claims.Where(c => c.Type == ClaimDeclaration.Role).Select(c => c.Value).ToList(),
            };

            return user;
        }

        public static long CalculateRefreshTokenRenewalTime(string tokenExpiresAt) => (DateTime.Parse(tokenExpiresAt).AddSeconds(-120)).ToUniversalTime().Ticks;
    }
}
