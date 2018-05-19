using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.SPA.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : BaseController
    {
        [HttpGet("[action]")]
        public IActionResult WeatherForecasts()
        {
            return Ok(5);
        }

    }
}
