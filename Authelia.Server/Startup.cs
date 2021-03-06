using Microsoft.EntityFrameworkCore;
using Authelia.Server.Configuration;
using Authelia.Server.Extensions;
using Authelia.Server.Exceptions;
using Authelia.Server.Helpers;
using Authelia.Database.Model;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Diagnostics;

namespace Authelia.Server
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            services.AddSwaggerGen();
            Setup.ConfigureServices(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            Setup.Configure(app);

            app.UseAutheliaExceptionHandler();
        }
    }
}
