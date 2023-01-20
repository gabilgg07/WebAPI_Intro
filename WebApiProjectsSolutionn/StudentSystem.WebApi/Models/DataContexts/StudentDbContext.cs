using System;
using Microsoft.EntityFrameworkCore;
using StudentSystem.WebApi.Models.Entities;

namespace StudentSystem.WebApi.Models.DataContexts
{
	public class StudentDbContext : DbContext
	{
		public StudentDbContext(DbContextOptions options)
			:base(options)
		{
		}

		public DbSet<Group> Groups { get; set; }

		public DbSet<Student> Students { get; set; }
    }
}

