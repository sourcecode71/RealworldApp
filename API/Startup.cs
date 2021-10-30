using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using API.Extensions;
using API.Services;
using Application.Core.Projects;
using Domain;
using MediatR;
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
using Persistance.Context;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            });

            services.AddMediatR(typeof(Create.Handler).Assembly);
            services.AddScoped<TokenService>();

            services.AddIdentity<Employee, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<Employee>>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(opt =>
                    {
                        opt.SaveToken = true;
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = key,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false
                        };
                    });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("IsAdmin", policy =>
            //    {
            //        policy.RequireClaim("role", "Admin").Build();
            //    });
            //    options.AddPolicy("IsEmployee", policy =>
            //    {
            //        policy.RequireClaim("role", "Employee").Build();
            //    });
            //});

            services.AddAuthorization();

            //services.AddIdentityServices(_config);
            // services.AddApplicationServices(_config);

            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowOrigin", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().Build();
                });
            });

            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));

            }).AddNewtonsoftJson(options =>
                            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowOrigin");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}