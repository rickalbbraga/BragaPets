using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BragaPets.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Teste V2");
        }
    }
}