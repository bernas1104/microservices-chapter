using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Shared.ServiceDiscovery;
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
            services.AddDbContext<UsersDbContext>(
                opt =>
                {
                    opt.UseNpgsql(Configuration.GetConnectionString(
                        "DefaultConnection"
                    ),
                    b => b.MigrationsAssembly("Users.Infra"));
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseConsul(Configuration);
        }
    }
}
