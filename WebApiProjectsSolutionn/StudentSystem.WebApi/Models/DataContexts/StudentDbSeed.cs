using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

