using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication2.Models;
using WebApplication2.Repositories;
using WebApplication2.Repositories.Interfaces;

namespace WebApplication2
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
            services.AddDbContext<AppDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

           // services.AddDatabaseDeveloperPageExceptionFilter();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.AccessDeniedPath = "/Instituicoes/AccessDenied/";
                options.LoginPath = "/Administradores/Login"; //Instituicoes/Login
            });


            services.AddControllersWithViews();


            //services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/AccessDenied");

            ////fornece uma instancia de HttpContextAcessor
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IInstituicaoRepository, InstituicaoRepository>();
            services.AddTransient<ICategoriaRepository, CategoriaRepository>();



            ////configura o uso da Sess?o
            //services.AddMemoryCache();
            //services.AddSession();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
