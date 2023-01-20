using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.WebApi.Models.Entities
{
    public class Group
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public virtual ICollection<Student> Students { get; set; }
	}
}

