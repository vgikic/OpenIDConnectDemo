using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.SPA.Controllers
{
    [Authorize]
    public class BaseController : Controller  {   }
}