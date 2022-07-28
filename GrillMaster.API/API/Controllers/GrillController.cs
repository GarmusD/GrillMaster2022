using GrillMaster.API.Models.Account;
using GrillMaster.Data.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrillMaster.API.Controllers
{
    [Route("api/grill")]
    [ApiController]
    public class GrillController : ControllerBase
    {
        ILogger<GrillController> _logger;

        public GrillController(ILogger<GrillController> logger)
        {
            _logger = logger;
        }

        private UserModel? GetCurrentUser()
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var claims = identity.Claims;
                return new()
                {
                    Username = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!,
                    Role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value!
                };
            }
            return null;
        }

        /// <summary>
        /// Optimize grill order.
        /// </summary>
        /// <param name="grillOrder">Grill order to optimize</param>
        /// <returns name="OptimizedOrder">Grill order optimized to grill pads</returns>
        /// <remarks>
        /// Sample request:
        /// POST /api/grill/optimize
        /// {
        ///   "grillSize":{"width":11, "height":22},
        ///   menuItems:[
        ///     {
        ///       "dimensions":{"width":3,"height":4},
        ///       "name":"MenuItemName",
        ///       "count":7}
        ///   ]
        /// }
        /// </remarks>
        /// <response code="200">Returns OptimizedOrder</response>
        /// <response code="401">If the user is not authorized to use this resource</response>
        [HttpPost("optimize")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Optimize([FromBody] GrillOrder grillOrder)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = new GrillOptimizer.GrillOptimizer(grillOrder).Optimize();
                return Ok(result);
            }
            return Unauthorized();
        }
    }
}
