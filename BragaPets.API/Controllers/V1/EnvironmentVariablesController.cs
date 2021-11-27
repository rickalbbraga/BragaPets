using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BragaPets.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/variables")]
    public class EnvironmentVariablesController : ControllerBase
    {
        private readonly ILogger<EnvironmentVariablesController> _logger;

        public EnvironmentVariablesController(ILogger<EnvironmentVariablesController> logger)
        {
            _logger = logger;
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var variables = new List<string>();
                variables.Add(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                variables.Add(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"));
                variables.Add(Environment.GetEnvironmentVariable("SqlServer"));
                
                _logger.LogInformation("Teste de log com logstash");
                
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