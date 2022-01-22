using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Authelia.Server.Requests.Entities;
using Authelia.Server.Validation;
using Authelia.Server.Security;
using Authelia.Database.Model;
using Microsoft.AspNetCore.Authentication;
using Authelia.Server.Authentication;
using Authelia.Server.Exceptions;
using Authelia.Server.Extensions;
using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Authelia.Server.Controllers
{
    [Route("api/[controller]")]
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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser user, [FromQuery]string returnUrl)
        {
            var validation = await userLoginValidator.ValidateAsync(user);

            if (!validation.IsValid)
                return BadRequest(validation.Adapt<ErrorResponse>()
                        .WithCode(ErrorCodes.C_InvalidUserLoginObject));

            try
            {
                var password = passwordSecurer.Secure(user.Password);
                var result = dbContext.Users.FirstOrDefault(u => (
                    u.UserName == user.UserName ||
                    u.UserMail == user.UserName ||
                    u.UserPhone == user.UserName) && u.UserPassword == password);

                if (result == null) return NotFound("user not registered");

                var claims = new Claim[] {
                    new Claim(ClaimConstants.Username, result.UserName),
                    new Claim(ClaimConstants.Email, result.UserMail ?? ""),
                    new Claim(ClaimConstants.Phone, result.UserPhone ?? ""),
                    new Claim(ClaimConstants.Verified, result.UserVerified.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                
                await HttpContext.SignInAsync(principal);

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
    }
}
