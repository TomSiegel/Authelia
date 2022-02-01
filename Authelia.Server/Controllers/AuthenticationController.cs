using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authelia.Server.Requests.Entities;
using Authelia.Server.Validation;
using Authelia.Server.Security;
using Authelia.Database.Model;
using Microsoft.AspNetCore.Authentication;
using Authelia.Server.Authentication;
using Authelia.Server.Exceptions;
using Authelia.Server.Extensions;
using Authelia.Server.Helpers;
using Mapster;
using Authelia.Server.Authorization;

namespace Authelia.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserLoginValidator userLoginValidator;
        private IPasswordSecurer passwordSecurer;
        private AutheliaDbContext dbContext;

        public AuthenticationController(UserLoginValidator userLoginValidator, IPasswordSecurer passwordSecurer, AutheliaDbContext dbContext)
        {
            this.userLoginValidator = userLoginValidator;
            this.passwordSecurer = passwordSecurer;
            this.dbContext = dbContext;
        }


        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user, [FromQuery]string returnUrl)
        {
            var validation = await userLoginValidator.ValidateAsync(user);

            if (!validation.IsValid)
                return BadRequest(validation.Adapt<ErrorResponse>()
                        .WithCode(ErrorCodes.C_InvalidUserLoginObject));

            try
            {
                var password = passwordSecurer.Secure(user.Password);
                var userResult = await dbContext.Users.FirstOrDefaultAsync(u => (
                    u.UserName == user.UserName ||
                    u.UserMail == user.UserName ||
                    u.UserPhone == user.UserName) && u.UserPassword == password);

                if (userResult == null) return NotFound("user not registered");

                await userResult.SignInAsync(HttpContext);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Adapt<ErrorResponse>()
                    .WithCode(ErrorCodes.S_UnknownServerError));
            }
        }

        [HttpGet("logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
           try
            {
                await HttpContext.SignOutAsync();
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Adapt<ErrorResponse>()
                    .WithCode(ErrorCodes.S_UnknownServerError));
            }

            return Ok();
        }

        [HttpGet("token"), Authorize]
        public async Task<IActionResult> CreateToken()
        {
            try
            {
                var id = HttpContext.User.GetClaim(ClaimConstants.UserIdentifier);
                var user = await dbContext.Users.SingleOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                    return NotFound("currently authenticated user is not registered");

                var timestamp = DateTime.UtcNow;
                var tokenId = DbHelpers.CreateGuid();
                var token = new UserToken()
                {
                    TokenCreatedUtc = timestamp,
                    TokenCreatorIp = HttpContext.GetUserIp(),
                    TokenExpiration = timestamp.AddHours(1),
                    UserId = user.UserId,
                    UserTokenId = passwordSecurer.Secure(tokenId)
                };

                await dbContext.UserTokens.AddAsync(token);
                await dbContext.SaveChangesAsync();

                return new JsonResult(token
                    .Adapt<UserTokenResponseDto>()
                    .WithToken(tokenId));
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Adapt<ErrorResponse>()
                    .WithCode(ErrorCodes.S_UnknownServerError));
            }
        }
    }
}
