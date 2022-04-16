using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistance.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Web.Services;

namespace Web.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration _config)
        {
            services.AddScoped<TokenService>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt =>
                    {
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

            services.AddIdentity<Employee, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<Employee>>();

            return services;
        }
    }
}