using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shared.ServiceDiscovery;
using Users.API.Middlewares;
using Users.API.Services;
using Users.API.Services.Interfaces;
using Users.Domain.Interfaces.MessageBus;
using Users.Domain.Interfaces.Repositories;
using Users.Infra.Context;
using Users.Infra.MessageBus;
using Users.Infra.Repositories;

namespace Users.API
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

            builder.AddNpgSql(
                Configuration.GetConnectionString("DefaultConnection"),
                tags: new[] { "self" }
            );

            builder.AddDbContextCheck<UsersDbContext>();

            services.AddDbContext<UsersDbContext>(
                opt =>
                {
                    opt.UseNpgsql(
                        Configuration.GetConnectionString(
                            "DefaultConnection"
                        ),
                        b => b.MigrationsAssembly("Users.Infra")
                    );
                }
            );

            services.AddMvc(
                options =>
                {
                    options.EnableEndpointRouting = false;
                }
            );

            services.AddControllers();
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "Users.API",
                            Version = "v1"
                        }
                    );
                }
            );

            services
                .AddConsulConfig(Configuration)
                .AddAutoMapper(typeof(Startup));

            services.AddScoped<UsersDbContext>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IMessageBusClient, RabbitMQClient>();
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
                        "Users.API v1"
                    )
                );
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<InstanceIdMiddleware>();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                }
            );

            app.UseConsul(Configuration);
        }
    }
}
