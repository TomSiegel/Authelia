using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authelia.Database.Model;
using Authelia.Server.Requests.Entities;
using Authelia.Server.Exceptions;
using Authelia.Server.Extensions;
using Authelia.Server.Validation;
using Mapster;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Authelia.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMetaController : ControllerBase
    {
        private readonly AutheliaDbContext dbContext;
        private readonly UserMetaCreateValidator createValidator;
        private readonly UserMetaUpdateValidator updateValidator;

        public UserMetaController(AutheliaDbContext dbContext, UserMetaCreateValidator createValidator, UserMetaUpdateValidator updateValidator)
        {
            this.dbContext = dbContext;
            this.createValidator = createValidator;
            this.updateValidator = updateValidator;
        }



        // GET: api/<UserMetaController>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var result = await dbContext.UserMeta.Where(x => x.UserId == id).ToListAsync();
            return new JsonResult(result);
        }

        // GET: api/<UserMetaController>
        [HttpGet("{id}/{name}")]
        public async Task<IActionResult> Get([FromRoute] string id,[FromRoute] string name)
        {
            var result = await dbContext.UserMeta.Where(x => x.UserId == id && x.UserMetaKey == name).SingleOrDefaultAsync();
            return new JsonResult(result);
        }


        // POST api/<UserMetaController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserMetaCreateRequest[] metaItems)
        {
            if (metaItems == null || metaItems.Length == 0)
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = "No meta items defined",
                    Code = ErrorCodes.C_EmptyEntityList
                });
            }

            foreach (var meta in metaItems)
            {
                var result = await createValidator.ValidateAsync(meta);

                if (!result.IsValid)
                    return BadRequest(result.Adapt<ErrorResponse>()
                        .WithData(meta)
                        .WithCode(ErrorCodes.C_InvalidUserMetaCreationObject));    
            }

            try
            {
                await dbContext.UserMeta.AddRangeAsync(metaItems.AdaptList<UserMetaCreateRequest, UserMetum>());
                await dbContext.SaveChangesAsync();

                return new JsonResult(metaItems);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = "Inserting meta items failed. Make sure you used valid user-ids and the meta item is unique.",
                    Data = ex.Entries,
                    Code = ErrorCodes.C_DatabaseItemInsertError
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Adapt<ErrorResponse>()
                   .WithCode(ErrorCodes.S_UnknownServerError));
            }
           
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post([FromRoute] string id, [FromBody] UserMetaCreateRequest[] metaItems)
        {
            if (metaItems != null)
            {
                foreach (var item in metaItems)
                {
                    item.UserId = id;
                }
            }

            return await Post(metaItems);
        }

        // PUT api/<UserMetaController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserMetaUpdateRequest[] metaItems)
        {
            if (metaItems == null || metaItems.Length == 0)
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = "No meta items defined",
                    Code = ErrorCodes.C_EmptyEntityList
                });
            }

            foreach (var meta in metaItems)
            {
                var result = await updateValidator.ValidateAsync(meta);

                if (!result.IsValid)
                    return BadRequest(result.Adapt<ErrorResponse>()
                        .WithData(meta)
                        .WithCode(ErrorCodes.C_InvalidUserMetaUpdateObject));
            }

            try
            {
                dbContext.UserMeta.UpdateRange(metaItems.AdaptList<UserMetaUpdateRequest, UserMetum>());
                await dbContext.SaveChangesAsync();

                return new JsonResult(metaItems);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = "Updating meta items failed. Make sure you used valid user-ids and the meta item is unique.",
                    Data = ex.Entries,
                    Code = ErrorCodes.C_DatabaseItemInsertError
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Adapt<ErrorResponse>()
                   .WithCode(ErrorCodes.S_UnknownServerError));
            }
        }

        // DELETE api/<UserMetaController>/5
        [HttpDelete("{id}/{name}")]
        public async Task<IActionResult> Delete([FromRoute] string id, [FromRoute] string name)
        {
            var entity = new UserMetum() { 
                UserId = id,
                UserMetaKey = name
            };
            
            dbContext.UserMeta.Remove(entity);
            var result = await dbContext.SaveChangesAsync();
             
            if (result == 0) return NotFound("no meta item was deleted");

            return Ok();
        }
    }
}
