using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Demo.API.Controllers
{
    [Route("api/[controller]")]
    public class DataController : BaseController
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetDataAsync()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            var data = await Task.Run(() => new string[] { "one", "two", "three" });
            return Ok(data);
        }

    }
}