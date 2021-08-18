using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Notifications.API.Consumers;
using Shared.ServiceDiscovery;

namespace Notifications.API
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
            var healthCheck = services.AddHealthChecksUI(
                setupSettings: setup =>
                {
                    setup.DisableDatabaseMigrations();
                    setup.MaximumHistoryEntriesPerEndpoint(6);
                }
            ).AddInMemoryStorage();

            var builder = services.AddHealthChecks();

            builder.AddProcessAllocatedMemoryHealthCheck(
                500 * 1024 * 1024,
                "Process Memory",
                tags: new[] { "self" }
            );

            builder.AddPrivateMemoryHealthCheck(
                500 * 1024 * 1024,
                "Private memory",
                tags: new[] { "self" }
            );

            services.AddControllers();
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "Notifications.API",
                            Version = "v1"
                        }
                    );
                }
            );

            services.AddHostedService<UserCreatedConsumer>();
            services.AddHostedService<UserUpdatedConsumer>();

            services.AddConsulConfig(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks(
                "/health",
                new HealthCheckOptions()
                {
                    AllowCachingResponses = false,
                    Predicate = r => r.Tags.Contains("self"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                }
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    c => c.SwaggerEndpoint(
                        "/swagger/v1/swagger.json",
                        "Notifications.API v1"
                    )
                );
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseConsul(Configuration);
        }
    }
}
