using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ModuleUser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public ActionResult Test(){
            return Ok("Hello World");
        }
    }
}