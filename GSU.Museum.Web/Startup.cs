using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Middleware;
using GSU.Museum.Web.Repositories;
using GSU.Museum.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace GSU.Museum.Web
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
            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddSingleton<IHallsRepository, HallsRepository>();
            services.AddSingleton<IStandsRepository, StandsRepository>();
            services.AddSingleton<IExhibitsRepository, ExhibitsRepository>();
            services.AddSingleton<IUsersRepository, UsersRepository>();

            services.AddSingleton<IHomeService, HomeService>();
            services.AddSingleton<IExhibitsService, ExhibitsService>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IFormFileToByteConverterService, FormFileToByteConverterService>();

            services.AddControllersWithViews();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

            app.Use((context, next) => {
                context.Request.Cookies.TryGetValue("GSU.Museum.Web.AccessToken", out string accessToken);
                context.Request.Cookies.TryGetValue("GSU.Museum.Web.RefreshToken", out string refreshToken);
                context.Request.Headers.Add("access_token", accessToken);
                context.Request.Headers.Add("refresh_token", refreshToken);
                return next.Invoke();
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
