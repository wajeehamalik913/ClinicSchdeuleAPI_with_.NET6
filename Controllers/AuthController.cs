#nullable disable
using ClinicApi.Interfaces;
using ClinicApi.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //Authentication interface for the api funtionality
        private readonly IAuth _Authenticate;

        //Constructor to initialize interface
        public AuthController(IAuth Authenticate)
        {
            _Authenticate = Authenticate;
        }

        //POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM registerVM)
        {

            //checking if request is valid
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest("Enter all required fields");
            //}

            //checking if user exists
            var userExists = await _Authenticate.Register(registerVM);
            if (userExists == null)
            {
                return BadRequest("User could not be created");
            }
            //retrning success 
            return Ok("User created");
        }

        //POST: api/Auth/login
        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {
            //checking if request is valid
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest("Enter all required fields");
            //}
          
            var tokenValue = await _Authenticate.Login(loginVM);
            //checking if Token is generated and valid
            if (tokenValue == null)
            {
                return Unauthorized();
            }
            return Ok(tokenValue);

        }

        //POST: api/Auth/refreshToken
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenVM refreshTokenVM)
        {
            //checking if request model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest("Enter all required fields");
            }
            var tokenValue = await _Authenticate.RefreshToken(refreshTokenVM);
            return Ok(tokenValue);

        }
    }
}
