using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentSystem.WebApi.Models.Entities;
using StudentSystem.WebApi.Models.Entity.Membership;

namespace StudentSystem.WebApi.Models.DataContexts
{
    public class StudentDbContext
        : IdentityDbContext<StudentSystemUser, StudentSystemRole, int, StudentSystemUserClaim, StudentSystemUserRole,
            StudentSystemUserLogin, StudentSystemRoleClaim, StudentSystemUserToken>
    {
		public StudentDbContext(DbContextOptions options)
			:base(options)
		{
		}

		public DbSet<Group> Groups { get; set; }

		public DbSet<Student> Students { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StudentSystemUser>(e =>
            {
                e.ToTable("Users", "Membership");
            });
            builder.Entity<StudentSystemRole>(e =>
            {
                e.ToTable("Roles", "Membership");
            });
            builder.Entity<StudentSystemUserRole>(e =>
            {
                e.ToTable("UserRoles", "Membership");
            });
            builder.Entity<StudentSystemUserClaim>(e =>
            {
                e.ToTable("UserClaims", "Membership");
            });
            builder.Entity<StudentSystemRoleClaim>(e =>
            {
                e.ToTable("RoleClaims", "Membership");
            });
            builder.Entity<StudentSystemUserLogin>(e =>
            {
                e.ToTable("UserLogins", "Membership");
            });
            builder.Entity<StudentSystemUserToken>(e =>
            {
                e.ToTable("UserTokens", "Membership");
            });
        }
    }
}

