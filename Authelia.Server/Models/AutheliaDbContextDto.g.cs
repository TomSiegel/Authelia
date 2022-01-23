using Authelia.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Authelia.Database.Model
{
    public partial class AutheliaDbContextDto
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserMetum> UserMeta { get; set; }
        public DatabaseFacade Database { get; set; }
        public ChangeTracker ChangeTracker { get; set; }
        public IModel Model { get; set; }
        public DbContextId ContextId { get; set; }
    }
}