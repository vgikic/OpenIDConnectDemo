using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [Authorize]
    public class BaseController : Controller {  }
}