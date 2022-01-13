﻿using Microsoft.AspNetCore.Mvc;
using Authelia.Database.Model;
using Authelia.Server.Validation;
using Authelia.Server.Helpers;
using Authelia.Server.Security;
using Authelia.Server.Exceptions;
using Authelia.Server.Extensions;
using Mapster;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Authelia.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AutheliaDbContext dbContext;
        private readonly CreateUserValidator createUserValidator;
        private readonly IPasswordSecurer passwordSecurer;

        public UsersController(AutheliaDbContext dbContext, CreateUserValidator createUserValidator, IPasswordSecurer passwordSecurer)
        {
            this.dbContext = dbContext;
            this.createUserValidator = createUserValidator;
            this.passwordSecurer = passwordSecurer;
        }


        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return dbContext.Users;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.UserId == id);

            if (user == null) return NotFound($"user with id {id} not found");

            return new JsonResult(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserDto[] users)
        {
            foreach (var user in users)
            {
                var result = await createUserValidator.ValidateAsync(user);

                if (!result.IsValid) 
                    return BadRequest(result.Adapt<ErrorResponse>()
                        .WithData(user)
                        .WithCode(ErrorCodes.C_InvalidUserCreationObject));

                user.UserId = DbHelpers.CreateGuid();
                user.UserCreated = DateTime.UtcNow;
                user.UserCreatorIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                user.UserVerified = 0;
                user.UserPassword = passwordSecurer.Secure(user.UserPassword);
            }

            try
            {
                foreach (var user in users)
                {
                    await dbContext.Users.AddAsync(user.Adapt<User>());
                }
  
                await dbContext.SaveChangesAsync();

                return new JsonResult(users);
            } catch(Exception ex)
            {
                return StatusCode(500, ex.Adapt<ErrorResponse>()
                    .WithCode(ErrorCodes.S_UnknownServerError));
            }
            
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }
    }
}