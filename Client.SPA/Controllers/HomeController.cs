using Client.SPA.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Client.SPA.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Access token that is used for API authorization
            // Needs to be set as Bearer token for each Ajax request
            var access_token = await HttpContext.GetTokenAsync("access_token");

            // Each ajax request needs to check if token is soot to be expired
            // so renew token method can be called in time
            var expires_at = await HttpContext.GetTokenAsync("expires_at");

            var user = new UserDto
            {
                Token = access_token,
                TokenExpirationTime = (DateTime.Parse(expires_at).AddSeconds(-120)).ToUniversalTime().Ticks
            };

            return View(user);
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        private async Task IsTokenExpired()
        {

            var expires_at = await HttpContext.GetTokenAsync("expires_at");

            if (string.IsNullOrWhiteSpace(expires_at)
                    || ((DateTime.Parse(expires_at).AddSeconds(-60)).ToUniversalTime()
                    < DateTime.UtcNow))

            {
                // Renew token here...
                //   accessToken = await RenewTokens();
            }
            else
            {
                // get access token here
                //accessToken = await currentContext.Authentication
                //    .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            }
        }
    }
}
