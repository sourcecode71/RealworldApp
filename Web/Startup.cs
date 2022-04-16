using Application.Core.Projects;
using AspNetCore.SassCompiler;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistance.Context;
using Web.Ioc;
using Web.Services;
using Web.Setting.GlobalExceptionHandling;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(ApiService));
            services.AddSingleton(typeof(ConverterService<string>));

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddMediatR(typeof(Create.Handler).Assembly);
            services.AddScoped<TokenService>();

#if DEBUG
            services.AddSassCompiler();
#endif

            ContainerSetup.Setup(services, Configuration);

            services.AddIdentity<Employee, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<Employee>>();

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //services.AddAuthentication(auth =>
            //{
            //    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //        .AddJwtBearer(opt =>
            //        {
            //            opt.SaveToken = true;
            //            opt.TokenValidationParameters = new TokenValidationParameters
            //            {
            //                ValidateIssuerSigningKey = true,
            //                IssuerSigningKey = key,
            //                ValidateIssuer = false,
            //                ValidateAudience = false,
            //                ValidateLifetime = false
            //            };
            //        });

            //services.AddAuthorization();

            //services.AddCors(opt =>
            //{
            //    opt.AddPolicy("AllowOrigin", policy =>
            //    {
            //        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().Build();
            //    });
            //});

            //services.AddSession(options =>
            //{
            //    options.Cookie.Name = "Project.Session";
            //    options.Cookie.IsEssential = true;
            //    options.IdleTimeout = TimeSpan.FromDays(7);
            //});
            //services.ConfigureApplicationCookie(options => options.LoginPath = "/home/login");

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie("Cookies", options =>
             {
                 options.LoginPath = "/Home/Login"; // using the AuthController instead
             });

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.ConfigureExceptionHandler();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
