using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentSystem.WebApi.Models.Entity.Membership;

namespace StudentSystem.WebApi.Models.DataContexts
{
	static public class StudentDbSeed
	{
		static public IApplicationBuilder Seed(this IApplicationBuilder app)
		{
			using(var scope = app.ApplicationServices.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<StudentDbContext>();

				db.Database.Migrate();

				InitGroups(db);
			}

			return app;
		}


        internal static IApplicationBuilder SeedMembership(this IApplicationBuilder app)
        {
            string superAdminRoleName = "SuperAdmin";
            string superAdminEmail = "aaliyeva0791@gmail.com";
            string superAdminPassword = "!2023@QabilCoder0707#";
            string superAdminName = "sadmin";

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<StudentSystemUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<StudentSystemRole>>();

                var hasRole = roleManager.RoleExistsAsync(superAdminRoleName).Result;

                if (!hasRole)
                {
                    roleManager.CreateAsync(new StudentSystemRole
                    {
                        Name = superAdminRoleName
                    }).Wait();
                }

                var admin = userManager.FindByEmailAsync(superAdminEmail).Result;

                if (admin == null)
                {
                    admin = new StudentSystemUser
                    {
                        Email = superAdminEmail,
                        UserName = superAdminName,
                        EmailConfirmed = true
                    };

                    userManager.CreateAsync(admin, superAdminPassword).Wait();
                }

                var isAdmin = userManager.IsInRoleAsync(admin, superAdminRoleName).Result;

                if (!isAdmin)
                {
                    userManager.AddToRoleAsync(admin, superAdminRoleName).Wait();
                }

            }

            return app;
        }

        private static void InitGroups(StudentDbContext db)
        {
			if (!db.Groups.Any())
            {
                db.Groups.Add(new Entities.Group
                {
                    Name = "P511"
                });
                db.Groups.Add(new Entities.Group
                {
                    Name = "P220"
                });
                db.Groups.Add(new Entities.Group
                {
                    Name = "P115"
                });

                db.SaveChanges();
            }
        }
    }
}

