using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    }
}
