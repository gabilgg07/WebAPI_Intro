using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentSystem.WebApi.AppCode.Providers;
using StudentSystem.WebApi.Models.DataContexts;
using StudentSystem.WebApi.Models.Entity.Membership;

namespace StudentSystem.WebApi
{
    public class Startup
    {
        readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {  // Register the Swagger generator, defining 1 or more Swagger documents

            // asagida yazilib bu
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //});

            services.AddControllers(cfg =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                cfg.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthentication();
            services.AddAuthorization(cfg =>
            {

                if (Program.policies != null)
                {
                    foreach (var policy in Program.policies)
                    {
                        cfg.AddPolicy(policy, p =>
                        {
                            p.RequireAssertion(handler =>
                            {
                                bool allow = handler.User.IsInRole("SuperAdmin")
                                || handler.User.HasClaim(m => m.Type.Equals(policy) && m.Value.Equals("1"));

                                return allow;
                            });
                        });
                    }
                }
            });

            services.AddRouting(cfg =>
            {
                cfg.LowercaseUrls = true;
            });

            services.AddDbContext<StudentDbContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("cStringMacLocal"));
                //cfg.UseSqlServer(configuration.GetConnectionString("cStringSmarterasp"));
            });

            services.AddIdentity<StudentSystemUser, StudentSystemRole>()
                .AddEntityFrameworkStores<StudentDbContext>();

            services.AddScoped<SignInManager<StudentSystemUser>>();
            services.AddScoped<UserManager<StudentSystemUser>>();
            services.AddScoped<RoleManager<StudentSystemRole>>();
            services.AddScoped<IClaimsTransformation, ApiClaimProvider>();

            services.AddCors(cfg =>
            {
                cfg.AddPolicy("all", p => p.AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader());
            });

            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JWT Token Authentication API",
                    Description = "ASP.NET Core 3.1 Web API"
                });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"])) //Configuration["JwtToken:SecretKey"]  
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.SeedMembership().Seed();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("all");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

