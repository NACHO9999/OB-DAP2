using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using ob.IBusinessLogic;
using ob.WebApi.Filters;
using ob.WebApi.DTOs;
using ob.IBusinessLogic;
using ob.WebApi.Filters;

namespace ob.WebApi.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    [ExceptionFilter]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserLoginModel userLoginModel)
        {
            var token = _sessionService.Authenticate(userLoginModel.Email, userLoginModel.Contrasena);
            return Ok(new { token = token });
        }




        [HttpDelete("logout")]
        public IActionResult Logout()
        {
            var authToken = Guid.Parse(Request.Headers["Authorization"]);
            _sessionService.Logout(authToken);
            return Ok("Logged out successfully.");
        }


    }
}