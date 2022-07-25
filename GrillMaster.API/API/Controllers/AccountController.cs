using GrillMaster.API.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GrillMaster.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IConfiguration config, ILogger<AccountController> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Authenticate user.
        /// </summary>
        /// <param name="userLogin">Json UserName and Password to login</param>
        /// <returns>Jwt token</returns>
        /// <remarks>
        /// Sample request:
        /// POST /api/user/login
        /// {
        ///   "Username": "username",
        ///   "Password": "password"
        /// }
        /// </remarks>
        /// <response code="200">Returns token</response>
        /// <response code="404">If the user with given username and password cannot be found</response>
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserLoginModel userLogin)
        {
            var user = Authenticate(userLogin);
            if(user != null)
            {
                _logger.LogDebug("User '{User}' authenticated. Generating token.", user.Username);
                string token = GenerateToken(user);
                return Ok(token);
            }
            _logger.LogDebug("User '{user}' not authenticated.", userLogin.Username);
            return NotFound("User not found.");
        }

        private UserModel? Authenticate(UserLoginModel userLogin)
        {
            var cfgUser = _config["AuthUser:Username"];
            var cfgPassword = _config["AuthUser:Password"];

            if (string.IsNullOrWhiteSpace(cfgUser) || string.IsNullOrWhiteSpace(cfgPassword)) return null;

            if (userLogin.Username.ToLower() == cfgUser.ToLower() && userLogin.Password == cfgPassword)
            {
                return new UserModel() { Username = userLogin.Username, Role = "griller" };
            }
            return null;
        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims, 
                expires: DateTime.Now.AddMinutes(15), 
                signingCredentials: credentials);

            string secToken = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogDebug("Generated token: {token}", secToken);
            return secToken;
        }

        
    }
}
