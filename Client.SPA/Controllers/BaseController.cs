using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.SPA.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BaseController : Controller { }    
}