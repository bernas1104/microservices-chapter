using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Users.API.Middlewares
{
    public class InstanceIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public InstanceIdMiddleware(
            RequestDelegate next,
            IConfiguration configuration
        )
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"ID : {_configuration.GetValue<string>("ConsulConfig:ServiceId")}");

            await _next(context);
        }
    }
}
