using System;
namespace Project.WebApi.Models
{
	public class Contact
	{
		static int counter = 1;
		public Contact()
		{
			this.Id = counter++;
		}

		public int Id { get; private set; }

		public string Name { get; set; }

		public string Phone{ get; set; }
    }
}

