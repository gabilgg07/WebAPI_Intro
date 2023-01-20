using System;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.WebApi.Models.Entities
{
	public class Student
	{
		public int Id { get; set; }

        [Required]
        public string Name { get; set; }

		public string Surname { get; set; }

		public int GroupId { get; set; }

		public virtual Group Group { get; set; }
    }
}

