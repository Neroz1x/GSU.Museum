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
using System.Reflection;
using System.IO;
using System;
using Microsoft.OpenApi.Models;

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GSU.Museum API", Version = "v1" });
                c.AddSecurityDefinition("X-API-KEY", new OpenApiSecurityScheme
                {
                    Description = "Api key needed to access the endpoints. X-API-KEY: U3VwZXJTZWNyZXRBcGlLZXkxMjM",
                    In = ParameterLocation.Header,
                    Name = "X-API-KEY",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "X-API-KEY",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "X-API-KEY"
                            },
                        },
                        new string[] {}
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GSU.Museum API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
