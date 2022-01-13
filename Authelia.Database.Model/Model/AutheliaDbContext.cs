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

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserMetum> UserMeta { get; set; }

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
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.UserName, "idx_user_name")
                    .IsUnique();

                entity.HasIndex(e => e.UserOrder, "idx_user_order");

                entity.Property(e => e.UserId)
                    .HasMaxLength(32)
                    .HasColumnName("user_id");

                entity.Property(e => e.UserCreated).HasColumnName("user_created");

                entity.Property(e => e.UserCreatorIp)
                    .HasMaxLength(45)
                    .HasColumnName("user_creator_ip");

                entity.Property(e => e.UserMail)
                    .IsRequired()
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

                entity.Property(e => e.UserVerified)
                    .HasColumnType("tinyint")
                    .HasColumnName("user_verified");
            });

            modelBuilder.Entity<UserMetum>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("user_meta");

                entity.HasIndex(e => new { e.UserId, e.UserMetaKey }, "idx_user_meta_key")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasMaxLength(32)
                    .HasColumnName("user_id");

                entity.Property(e => e.UserMetaKey)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("user_meta_key");

                entity.Property(e => e.UserMetaValue)
                    .HasMaxLength(256)
                    .HasColumnName("user_meta_value");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
