using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using StudentSystem.WebApi.Models.DataContexts;

namespace StudentSystem.WebApi.AppCode.Providers
{
	public class ApiClaimProvider : IClaimsTransformation
	{
        readonly StudentDbContext db;
        public ApiClaimProvider(StudentDbContext db)
        {
            this.db = db;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                var idString = principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

                if (int.TryParse(idString, out int userId))
                {
                    var claims = await db.UserClaims.Where(c => c.UserId == userId)
                        .ToListAsync();

                    foreach (var item in claims)
                        (principal.Identity as ClaimsIdentity).AddClaim(new Claim(item.ClaimType, item.ClaimValue));
                }
            }

            return principal;
        }
    }
}

