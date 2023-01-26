using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentSystem.WebApi.Models.DataContexts;
using StudentSystem.WebApi.Models.Entity.Membership;
using StudentSystem.WebApi.Models.ViewModels;

namespace StudentSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        readonly SignInManager<StudentSystemUser> signInManager;
        readonly UserManager<StudentSystemUser> userManager;
        readonly StudentDbContext db;
        readonly IConfiguration config;

        public AccountController(SignInManager<StudentSystemUser> signInManager,
            UserManager<StudentSystemUser> userManager,
            StudentDbContext db, IConfiguration config)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.db = db;
            this.config = config;
        }

        [HttpPost("/api/login")]
        [AllowAnonymous]
        public async Task<IActionResult> SigninAsync([FromBody] SignInModel user)
        {

            var foundUser = await userManager.FindByEmailAsync(user.UserName);

            if (foundUser == null)
            {
                ModelState.AddModelError("UserName", "The username or password is incorrect");

                goto finish;
            }

            var checkResult = await signInManager.CheckPasswordSignInAsync(foundUser, user.Password, false);

            if (!checkResult.Succeeded)
            {
                ModelState.AddModelError("UserName", "The username or password is incorrect");

                goto finish;
            }

            if (!foundUser.EmailConfirmed)
            {
                ModelState.AddModelError("UserName", "Please, confirm your email address");

                goto finish;
            }

            var signinResult = await signInManager.PasswordSignInAsync(foundUser, user.Password, true, true);

            if (!signinResult.Succeeded)
            {
                ModelState.AddModelError("UserName", "The username or password is incorrect");

                goto finish;
            }

        finish:

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string issuer = config["Jwt:Issuer"];
            string audience = config["Jwt:Audience"];
            string secret = config["Jwt:Secret"];

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, foundUser.Id.ToString()));

            var token = new JwtSecurityToken(issuer,
                audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: credentials);

            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                error = false,
                token = tokenStr
            });

        }

         
    }
}

