using Microsoft.OpenApi.Models;
using Pleromi.BLS;
using Pleromi.DAL;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Pleromi.DAL.DatabaseContext;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet;

namespace Pleromi.Api
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
            services.AddSingleton<IManagedMqttClient>(provider =>
            {
                var mqttClientFactory = new MqttFactory();
                return mqttClientFactory.CreateManagedMqttClient();
            });

            services.AddDbContext<ElectralysisDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ElectralysisDb")));

            services.AddCors(options => {
                options.AddPolicy(name: "CORS_Everyone", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddPleromiDal(Configuration);
            services.AddPleromiBLS();
            services.AddControllers();
            services.AddMemoryCache();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Swagger API",
                    Description = "A simple example ASP.NET Core Web API"
                });
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new MediaTypeApiVersionReader("version"),
                    new HeaderApiVersionReader("X-Version")
                );
                options.ReportApiVersions = true;
            });

            services.AddHealthChecks().AddCheck<ApiHealthCheck>(
                "Macro Indicator MicroService Health Check",
                tags: new string[] { "Indicator health api" }
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CORS_Everyone");

            app.UseHttpsRedirection();
            app.UseRouting();
            app.AddMacroShared(Configuration);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger API V1");
            });

            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}
