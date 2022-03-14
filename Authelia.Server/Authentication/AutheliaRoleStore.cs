using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Authelia.Server.Exceptions;
using Authelia.Server.Helpers;
using Authelia.Database.Model;
using Mapster;

namespace Authelia.Server.Authentication
{
    public class AutheliaRoleStore : IRoleStore<AutheliaRole>
    {
        private AutheliaDbContext autheliaDbContext;

        public AutheliaRoleStore(AutheliaDbContext autheliaDbContext)
        {
            this.autheliaDbContext = autheliaDbContext;
        }

        public async Task<IdentityResult> CreateAsync(AutheliaRole role, CancellationToken cancellationToken)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.RoleName)) 
                return IdentityResult.Failed(new IdentityError() { Code = ErrorCodes.S_ArgumentNull, Description = "no roleName provided" });

           try
            {
                await autheliaDbContext.Roles.AddAsync(new Role()
                {
                    RoleName = role.RoleName,
                    RoleId = DbHelpers.CreateGuid()
                });
                await autheliaDbContext.SaveChangesAsync();
            } catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Code = ErrorCodes.S_DatabaseInsert, Description = ex.Message });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AutheliaRole role, CancellationToken cancellationToken)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.RoleId))
                return IdentityResult.Failed(new IdentityError() { Code = ErrorCodes.S_ArgumentNull, Description = "no roleId provided" });

            try { 
            autheliaDbContext.Roles.Remove(role.Adapt<Role>());
            await autheliaDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Code = ErrorCodes.S_DatabaseInsert, Description = ex.Message });
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            autheliaDbContext = null;
        }

        public async Task<AutheliaRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var result = await autheliaDbContext.Roles.FirstOrDefaultAsync(x => x.RoleId == roleId, cancellationToken);

            if (result == null) return null;

            return result.Adapt<AutheliaRole>();
        }

        public async Task<AutheliaRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var result = await autheliaDbContext.Roles.FirstOrDefaultAsync(x => x.RoleId == normalizedRoleName, cancellationToken);

            if (result == null) return null;

            return result.Adapt<AutheliaRole>();
        }

        public Task<string> GetNormalizedRoleNameAsync(AutheliaRole role, CancellationToken cancellationToken)
        {
           return GetRoleNameAsync(role, cancellationToken);
        }

        public async Task<string> GetRoleIdAsync(AutheliaRole role, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(role.RoleName) && string.IsNullOrWhiteSpace(role.RoleId))
                return (await FindByNameAsync(role.RoleName, cancellationToken))?.RoleId;

            return role.RoleId;
        }

        public async Task<string> GetRoleNameAsync(AutheliaRole role, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(role.RoleName) && !string.IsNullOrWhiteSpace(role.RoleId))
                return (await FindByIdAsync(role.RoleId, cancellationToken))?.RoleName;

            return role.RoleName;
        }

        public Task SetNormalizedRoleNameAsync(AutheliaRole role, string normalizedName, CancellationToken cancellationToken)
        {
            return SetRoleNameAsync(role, normalizedName, cancellationToken);
        }

        public async Task SetRoleNameAsync(AutheliaRole role, string roleName, CancellationToken cancellationToken)
        {
            var result = await autheliaDbContext.Roles.FirstOrDefaultAsync(x => x.RoleId == role.RoleId);

            if (result == null) return;
            result.RoleName = roleName;
            role.RoleName =  roleName;

            autheliaDbContext.Roles.Update(result);
            await autheliaDbContext.SaveChangesAsync();
        }

        public async Task<IdentityResult> UpdateAsync(AutheliaRole role, CancellationToken cancellationToken)
        {
            var r1 = await DeleteAsync(role, cancellationToken);
            var r2 = await CreateAsync(role, cancellationToken);

            if (!r1.Succeeded || !r2.Succeeded)
                return IdentityResult.Failed(r1.Errors.Concat(r2.Errors).ToArray());

            return IdentityResult.Success;
        }
    }
}
