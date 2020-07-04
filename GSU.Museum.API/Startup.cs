using GSU.Museum.API.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using GSU.Museum.API.Data;
using GSU.Museum.API.Data.Repositories;
using GSU.Museum.API.Services;

namespace GSU.Museum.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddControllers();

            services.AddSingleton<IHallsRepository, HallsRepository>();
            services.AddSingleton<IStandsRepository, StandsRepository>();
            services.AddSingleton<IExhibitsRepository, ExhibitsRepository>();

            services.AddSingleton<IHallsService, HallsService>();
            services.AddSingleton<IStandsService, StandsService>();
            services.AddSingleton<IExhibitsService, ExhibitsService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
