using Client.SPA.DTO;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Shared;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Client.SPA.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : BaseController
    {
        [HttpGet("[action]")]
        public IActionResult WeatherForecasts()
        {
            var isAdmin = User.HasClaim("role", "Admin");

            if (!isAdmin)
                return NotFound();


            return Ok(5);
        }

        [HttpGet("[action]")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync("oidc");
        }

        [HttpGet("[action]")]
        public async Task<UserDto> RenewTokens()
        {
            // get the metadata
            var discoveryClient = await DiscoveryClient.GetAsync(Parties.AuthorityUrl);

            // create a new token client to get new tokens
            var tokenClient = new TokenClient(discoveryClient.TokenEndpoint, "SPA_APP", "password");

            // get the saved refresh token
            var currentRefreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            // refresh the tokens
            var tokenResult = await tokenClient.RequestRefreshTokenAsync(currentRefreshToken);

            if (!tokenResult.IsError)
            {
                // get auth info
                var authenticateInfo = await AuthenticationHttpContextExtensions.AuthenticateAsync(HttpContext);
                // create a new value for expires_at, and save it
                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);

                authenticateInfo.Properties.UpdateTokenValue("expires_at", expiresAt.ToString("o", CultureInfo.InvariantCulture));
                authenticateInfo.Properties.UpdateTokenValue(OpenIdConnectParameterNames.AccessToken, tokenResult.AccessToken);
                authenticateInfo.Properties.UpdateTokenValue(OpenIdConnectParameterNames.RefreshToken, tokenResult.RefreshToken);

                // we're signing in again with the new values.  
                await HttpContext.SignInAsync("Cookies", authenticateInfo.Principal, authenticateInfo.Properties);

                // return the new access token 
                return new UserDto
                {
                    Token = tokenResult.AccessToken,
                    TokenExpirationTime = expiresAt.ToUniversalTime().Ticks
                };
            }
            else
            {
                throw new Exception("Problem encountered while refreshing tokens.",
                    tokenResult.Exception);
            }
        }

    }
}
