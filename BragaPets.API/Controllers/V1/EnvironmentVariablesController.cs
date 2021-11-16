using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BragaPets.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/variables")]
    public class EnvironmentVariablesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var variables = new List<string>();
                variables.Add(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                variables.Add(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"));
                
                return Ok(variables);
            }
            catch (Exception e)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                return Created("teste-com-fernando", new { Nome = "Fernando" });
            }
            catch (Exception e)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }
    }
}