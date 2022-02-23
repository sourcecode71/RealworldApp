using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PMG.Data.Repository.Projects;
using System;

namespace Web.Ioc
{
    public static class ContainerSetup
    {
        internal static void Setup(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureInject(services);
        }

        private static void ConfigureInject(IServiceCollection services)
        {
            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        }
    }
}
