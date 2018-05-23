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
    public class SpaApiController : BaseController
    {
        [HttpGet("[action]")]
        public IActionResult GetClientSpaProtectedApiDataAsync()
        {


            return Ok(5);
        }

 
        [HttpGet("[action]")]
        public async Task LogoutAsync()
        {

            /// <summary>
            /// Reference and refresh token invalidation exampele
            /// Only works if other refresh token configuration in API and IDP is set up properly
            /// </summary>
            //var discoveryClient = await DiscoveryClient.GetAsync(Parties.AuthorityUrl);
            //var revocationClient = new TokenRevocationClient(discoveryClient.RevocationEndpoint, Parties.WebClientId, "password");
            //var accessTokenToRevoke = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            //if (!string.IsNullOrEmpty(accessTokenToRevoke))
            //{
            //    var response = await revocationClient.RevokeAccessTokenAsync(accessTokenToRevoke);
            //    if (response.IsError)
            //        throw new Exception("Problem while revoking access token", response.Exception);
            //}
            //var refreshTokenToRevoke = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            //if (!string.IsNullOrEmpty(refreshTokenToRevoke))
            //{
            //    var response = await revocationClient.RevokeRefreshTokenAsync(refreshTokenToRevoke);
            //    if (response.IsError)
            //        throw new Exception("Problem while revoking refresh token", response.Exception);
            //}


            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync("oidc");
        }

        /// <summary>
        /// Refreshes token expiration time.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<UserDto> RenewTokensAsync()
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
                    TokenRenewalTime = expiresAt.ToUniversalTime().Ticks
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
