using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Shared;

namespace Demo.API.Controllers
{
    [Route("api/[controller]")]
    public class DataController : BaseController
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetDataAsync()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var userRole = User.HasClaim(ClaimDeclaration.Role, RoleType.User);

            var data = await Task.Run(() => new string[] { "one", "two", "three" });
            return Ok(data);
        }

        [HttpGet("[action]")]
        [Authorize(Policy = PolicyDeclaration.IsPremiumSubscriber)]
        public async Task<IActionResult> GetPolicyProtectedDataAsync()
        {

            // Example of non-attribute policy check
            //var isAuthorized = await authorizationService.AuthorizeAsync(User, "IsPremiumSubscriber");
            var data = await Task.Run(() => "TOP SECRET!");
            return Ok(data);
        }

        [HttpGet("[action]/{id}")]
        [Authorize(Policy = PolicyDeclaration.MustOwnRecord)]
        public async Task<IActionResult> GetCustomRequirementProtectedDataAsync(int id)
        {
            var data = await Task.Run(() => "!!!!!");
            return Ok(data);
        }

    }
}