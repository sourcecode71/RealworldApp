using Application.Core.Projects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistance.Context;

namespace Web.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration _config)
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

            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowOrigin", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().Build();
                });
            });

            return services;
        }
    }
}