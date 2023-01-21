using System;
using Microsoft.AspNetCore.Identity;

namespace StudentSystem.WebApi.Models.Entity.Membership
{
	public class StudentSystemUserToken:IdentityUserToken<int>
    {
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public DateTime? DeletedDate { get; set; }
    }
}

