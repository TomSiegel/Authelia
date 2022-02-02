using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authelia.Database.Model;
using Authelia.Server.Requests.Entities;
using Authelia.Server.Validation;
using Authelia.Server.Exceptions;
using Authelia.Server.Extensions;
using Authelia.Server.Helpers;
using Authelia.Server.Security;
using Authelia.Server.Authorization;
using Mapster;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Authelia.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSetupController : ControllerBase
    {
        private readonly AutheliaDbContext dbContext;
        private readonly IPasswordSecurer passwordSecurer;
        private readonly AdminCreateValidator createAdminValidator;


        public AdminSetupController(AutheliaDbContext dbContext, AdminCreateValidator createAdminValidator, IPasswordSecurer passwordSecurer)
        {
            this.dbContext = dbContext;
            this.createAdminValidator = createAdminValidator;
            this.passwordSecurer = passwordSecurer;
        }

        // GET: api/<AdminSetupController>
        [HttpGet, AllowAnonymous, Role("admin")]
        public async Task<AdminSetupResponse> Get()
        {
            var count = await dbContext.Users.Where(x => x.UserIsAdmin == 1).CountAsync();

            return new AdminSetupResponse() {
                Count = count
            };
        }

        // POST api/<AdminSetupController>
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] AdminCreateRequest admin)
        {
            if (admin == null) return BadRequest(new ErrorResponse()
            {
                Code = ErrorCodes.C_NullEntity,
                Message = "empty admin object"
            });

            var result = await createAdminValidator.ValidateAsync(admin);

            if (!result.IsValid)
                return BadRequest(result.Adapt<ErrorResponse>()
                    .WithData(admin)
                    .WithCode(ErrorCodes.C_InvalidAdminCreationObject));

            var status = await Get();

            if (status.HasAdmins) return BadRequest(new ErrorResponse()
            {
                Code = ErrorCodes.C_AdminAlreadyExists,
                Message = "there is already an existing admin. To create additional admins use the default user-endpoint and set them as admins."
            });

            admin.UserPassword = passwordSecurer.Secure(admin.UserPassword);

            var user = admin.Adapt<User>();

            user.UserId = DbHelpers.CreateGuid();
            user.UserCreatorIp = HttpContext.GetUserIp();
            user.UserIsAdmin = 1;
            user.UserVerified = 0;
            user.UserCreatedUtc = DateTime.UtcNow;
            user.UserDeletedUtc = null;
            user.UserMail = null;
            user.UserPhone = null;

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
