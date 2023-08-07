using Microsoft.AspNetCore.Mvc;
using StoreAdmin.Core.BusinessInterfaces;
using StoreAdmin.Core.DTOs;

namespace StoreAdmin.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersAuthController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;

        public UsersAuthController(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO request)
        {
            var token = _userAuthService.AuthenticateUser(request.username, request.Password);

            if (token == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            
            return Ok(new { Token = token, Message = "Login successful!" });
        }
    }
}
