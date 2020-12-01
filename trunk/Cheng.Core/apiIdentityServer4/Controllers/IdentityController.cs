using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace apiIdentityServer4.Controllers
{
    public class IdentityController : ControllerBase
    {
        [HttpGet("gettest")]
        [Authorize]
        public IActionResult Get()
        {
            return Content("gettest");
        }

        [HttpGet("getone")]
        public IActionResult GetOne()
        {
            return Content("getone");
        }
    }
}
