using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Authelia.Server.Exceptions;
using Authelia.Server.Helpers;
using Authelia.Database.Model;
using Mapster;


namespace Authelia.Server.Authentication
{
    public class AutheliaUserStore : IUserStore<AutheliaUser>, IUserClaimStore<AutheliaUser>, IUserRoleStore<AutheliaUser>
    {
        private HashSet<AutheliaUser> autheliaUsers = new HashSet<AutheliaUser>();
        private Dictionary<AutheliaUser, List<string>> autheliaUserRoles = new Dictionary<AutheliaUser, List<string>>();
        private Dictionary<AutheliaUser, List<Claim>> autheliaUserClaims = new Dictionary<AutheliaUser, List<Claim>>();

        private readonly AutheliaDbContext autheliaDbContext;
        private readonly AutheliaRoleStore autheliaRoleStore;

        public AutheliaUserStore(AutheliaDbContext autheliaDbContext)
        {
            this.autheliaDbContext = autheliaDbContext;
        }

        public async Task AddClaimsAsync(AutheliaUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var metaInfos = claims.Select(x => new UserMetum() { UserId = user.User.UserId, UserMetaKey = x.Type, UserMetaValue = x.Value });
            await autheliaDbContext.UserMeta.AddRangeAsync(metaInfos, cancellationToken);
            await autheliaDbContext.SaveChangesAsync();
        }

        public async Task AddToRoleAsync(AutheliaUser user, string roleName, CancellationToken cancellationToken)
        {
            await autheliaDbContext.UserRoles.AddAsync(new UserRole() { 
                UserId = user.User.UserId, 
                RoleId = await autheliaRoleStore.GetRoleIdAsync(new AutheliaRole() { RoleName = roleName}, cancellationToken) 
            });
            await autheliaDbContext.SaveChangesAsync();
        }

        public Task<IdentityResult> CreateAsync(AutheliaUser user, CancellationToken cancellationToken)
        {
            if (user == null || user.User == null) return Task.FromResult(IdentityResult.Failed(new IdentityError()
            {
                Code = ErrorCodes.S_ArgumentNull,
                Description = $"user is null"
            }));

            if (!autheliaUsers.Add(user)) return Task.FromResult(IdentityResult.Failed(new IdentityError()
            {
                Code = ErrorCodes.S_UserStoreAlreadyCreated,
                Description = $"the store for the user {user.User.UserName} is already registered"
            }));

            autheliaUserRoles.Add(user, GetUserRoles(user));
            autheliaUserClaims.Add(user, GetUserClaims(user));

            return Task.FromResult(IdentityResult.Success);
        }


        public Task<IdentityResult> DeleteAsync(AutheliaUser user, CancellationToken cancellationToken)
        {
            autheliaUsers.Remove(user);
            autheliaUserClaims.Remove(user);
            autheliaUserRoles.Remove(user);

            return Task.FromResult<IdentityResult>(IdentityResult.Success);
        }

        public void Dispose()
        {
            autheliaUsers = null;
            autheliaUserRoles = null;
            autheliaUserClaims = null;
        }

        public Task<AutheliaUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(autheliaUsers.FirstOrDefault(x => x.User.UserId == userId));
        }

        public Task<AutheliaUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.FromResult(autheliaUsers.FirstOrDefault(x => x.User.UserName == normalizedUserName));
        }

        public Task<IList<Claim>> GetClaimsAsync(AutheliaUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult((IList<Claim>)autheliaUserClaims[user]);
        }

        public Task<string> GetNormalizedUserNameAsync(AutheliaUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.User.UserName);
        }

        public Task<IList<string>> GetRolesAsync(AutheliaUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult((IList<string>)autheliaUserRoles[user]);
        }

        public Task<string> GetUserIdAsync(AutheliaUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.User.UserId);
        }

        public Task<string> GetUserNameAsync(AutheliaUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.User.UserName);
        }

        public Task<IList<AutheliaUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<AutheliaUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsInRoleAsync(AutheliaUser user, string roleName, CancellationToken cancellationToken)
        {
            if (roleName == null) return false;
            var list = await GetRolesAsync(user, cancellationToken);

            if (list == null) return false;

            return list.Contains(roleName);
        }

        public async Task RemoveClaimsAsync(AutheliaUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            if (claims == null) return;
            var list = await GetClaimsAsync(user, cancellationToken);

            if (list == null) return;

            foreach (var item in claims) list.Remove(item);
        }

        public async Task RemoveFromRoleAsync(AutheliaUser user, string roleName, CancellationToken cancellationToken)
        {
            if (roleName == null) return;
            var list = await GetRolesAsync(user, cancellationToken);

            if (list == null) return;

            list.Remove(roleName);
        }

        public async Task ReplaceClaimAsync(AutheliaUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            if (claim == null || newClaim == null) return;
            var list = await GetClaimsAsync(user, cancellationToken);

            if (list == null) return;

            list.Remove(claim);
            list.Add(newClaim);
        }

        public Task SetNormalizedUserNameAsync(AutheliaUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(AutheliaUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(AutheliaUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }




        private List<string> GetUserRoles(AutheliaUser user)
        {
            if (user?.User?.UserRoles != null && user.User.UserRoles.Count > 0)
            {
                return new List<string>(user.User.UserRoles.Where(x => x.Role != null).Select(x => x.Role.RoleName));
            }

            return new List<string>();
        }

        private List<Claim> GetUserClaims(AutheliaUser user)
        {
            return new List<Claim>();
        }
    }
}
