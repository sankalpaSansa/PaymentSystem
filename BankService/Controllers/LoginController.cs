using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Service.Interfaces;

namespace PaymentSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        IUserService _service;
        public LoginController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("authenticate")]
        public ActionResult Authenticate(string UserName)
        {
            var userDto = _service.Authenticate(UserName);
            return Ok(userDto);
        }

    }
}
