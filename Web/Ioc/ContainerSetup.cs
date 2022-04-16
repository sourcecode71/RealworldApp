using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PMG.Data.Repository.Employee;
using PMG.Data.Repository.PayInvoice;
using PMG.Data.Repository.Projects;

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
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        }
    }
}
