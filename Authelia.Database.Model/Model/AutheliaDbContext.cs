using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Authelia.Database.Model
{
    public partial class AutheliaDbContext : DbContext
    {
        public AutheliaDbContext()
        {
        }

        public AutheliaDbContext(DbContextOptions<AutheliaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserMetum> UserMeta { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("Server=127.0.0.1;Database=authelia-dev;Uid=root;Pwd=bakterie;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasIndex(e => e.RoleName, "role_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.RoleId)
                    .HasMaxLength(32)
                    .HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("role_name");

                entity
                    .HasMany(x => x.UserRoles)
                    .WithOne(x => x.Role);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.UserName, "idx_user_name")
                    .IsUnique();

                entity.HasIndex(e => e.UserOrder, "idx_user_order");

                entity.HasIndex(e => e.UserMail, "user_mail_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.UserPhone, "user_phone_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasMaxLength(32)
                    .HasColumnName("user_id");

                entity.Property(e => e.UserCreatedUtc).HasColumnName("user_created_utc");

                entity.Property(e => e.UserCreatorIp)
                    .HasMaxLength(45)
                    .HasColumnName("user_creator_ip");

                entity.Property(e => e.UserDeletedUtc).HasColumnName("user_deleted_utc");

                entity.Property(e => e.UserIsAdmin)
                    .HasColumnType("tinyint")
                    .HasColumnName("user_is_admin");

                entity.Property(e => e.UserMail)
                    .HasMaxLength(128)
                    .HasColumnName("user_mail");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserOrder)
                    .HasColumnType("int unsigned")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_order");

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("user_password");

                entity.Property(e => e.UserPhone)
                    .HasMaxLength(15)
                    .HasColumnName("user_phone");

                entity.Property(e => e.UserVerified)
                    .HasColumnType("tinyint")
                    .HasColumnName("user_verified");

                entity
                    .HasMany(x => x.UserRoles)
                    .WithOne(x => x.User);
            });

            modelBuilder.Entity<UserMetum>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.UserMetaKey })
                    .HasName("PRIMARY");

                entity.ToTable("user_meta");

                entity.HasIndex(e => new { e.UserId, e.UserMetaKey }, "idx_user_meta_key")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasMaxLength(32)
                    .HasColumnName("user_id");

                entity.Property(e => e.UserMetaKey)
                    .HasMaxLength(50)
                    .HasColumnName("user_meta_key");

                entity.Property(e => e.UserMetaValue)
                    .HasMaxLength(256)
                    .HasColumnName("user_meta_value");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserMeta)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_META_USER");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PRIMARY");

                entity.ToTable("user_roles");

                entity.HasIndex(e => e.RoleId, "FK_UR_TO_ROLES_idx");

                entity.Property(e => e.UserId)
                    .HasMaxLength(32)
                    .HasColumnName("user_id");

                entity.Property(e => e.RoleId)
                    .HasMaxLength(32)
                    .HasColumnName("role_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UR_TO_ROLES");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UR_TO_USERS");
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.ToTable("user_tokens");

                entity.HasIndex(e => e.UserId, "FK_TOKEN_USER_idx");

                entity.Property(e => e.UserTokenId)
                    .HasMaxLength(256)
                    .HasColumnName("user_token_id");

                entity.Property(e => e.TokenCreatedUtc).HasColumnName("token_created_utc");

                entity.Property(e => e.TokenCreatorIp)
                    .HasMaxLength(45)
                    .HasColumnName("token_creator_ip");

                entity.Property(e => e.TokenExpiration).HasColumnName("token_expiration");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_TOKEN_USER");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
