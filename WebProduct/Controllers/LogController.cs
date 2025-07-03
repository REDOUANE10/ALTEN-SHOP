using Microsoft.AspNetCore.Mvc;

namespace Api_Store.Controllers
    {
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
        {
        [HttpGet("error")]
        public IActionResult GenerateError()
            {
            throw new Exception("Erreur de test volontaire !");
            }

        [HttpGet("ok")]
        public IActionResult GetOk()
            {
            return Ok("Tout fonctionne !");
            }
        }
    }
