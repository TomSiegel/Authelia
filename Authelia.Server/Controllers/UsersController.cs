using Microsoft.AspNetCore.Mvc;
using Authelia.Database.Model;
using Authelia.Server.Validation;
using Authelia.Server.Helpers;
using Authelia.Server.Security;
using Authelia.Server.Exceptions;
using Authelia.Server.Extensions;
using Authelia.Server.Authorization;
using Mapster;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Authelia.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AutheliaDbContext dbContext;
        private readonly UserCreateValidator createUserValidator;
        private readonly IPasswordSecurer passwordSecurer;

        public UsersController(AutheliaDbContext dbContext, UserCreateValidator createUserValidator, IPasswordSecurer passwordSecurer)
        {
            this.dbContext = dbContext;
            this.createUserValidator = createUserValidator;
            this.passwordSecurer = passwordSecurer;
        }


        // GET: api/<UsersController>
        [HttpGet, Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await dbContext.Users.Where(x => x.UserDeletedUtc == null).ToListAsync();

            return new JsonResult(result.Select(x => x.Adapt<UserSafeDto>()));
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(x => x.UserId == id);

            if (user == null) return NotFound($"user with id {id} not found");

            return new JsonResult(user.Adapt<UserSafeDto>());
        }

        // POST api/<UsersController>
        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody] UserDto[] users)
        {
            foreach (var user in users)
            {
                var result = await createUserValidator.ValidateAsync(user);

                if (!result.IsValid) 
                    return BadRequest(result.Adapt<ErrorResponse>()
                        .WithData(user)
                        .WithCode(ErrorCodes.C_InvalidUserCreationObject));

                user.UserId = DbHelpers.CreateGuid();
                user.UserCreatedUtc = DateTime.UtcNow;
                user.UserCreatorIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                user.UserVerified = 0;
                user.UserPassword = passwordSecurer.Secure(user.UserPassword);
                user.UserMail = user.UserMail.RemoveWhitespace();
                user.UserPhone = user.UserPhone.RemoveWhitespace();
            }

            try
            {
                foreach (var user in users)
                {
                    await dbContext.Users.AddAsync(user.Adapt<User>());
                }
  
                await dbContext.SaveChangesAsync();

                return new JsonResult(users.Select(x => x.Adapt<UserSafeDto>()));
            } catch(Exception ex)
            {
                return StatusCode(500, ex.Adapt<ErrorResponse>()
                    .WithCode(ErrorCodes.S_UnknownServerError));
            }
            
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(x => x.UserId == id);

            if (user == null) return NotFound($"user with id {id} not found");

            user.UserDeletedUtc = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return new JsonResult(user);
        }
    }
}
