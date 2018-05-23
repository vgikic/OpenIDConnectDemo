using Client.SPA.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Client.SPA.Helpers;

namespace Client.SPA.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await Helpers.Helpers.GetUserInfoAsync(User, HttpContext);
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
