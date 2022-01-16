using Microsoft.EntityFrameworkCore;
using Authelia.Server.Configuration;
using Authelia.Server.Extensions;
using Authelia.Server.Exceptions;
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
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerGen();
            services.AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>(scan =>
                {
                    return true;
                }, ServiceLifetime.Singleton);
            });
            services.AddDbContext<AutheliaDbContext>(options =>
            {
                var settings = Configuration.GetSection("DbConnection").Get<DbConnectionOptions>();
                
                options.UseMySQL(settings.GetConnectionString());
            });
            services.AddSingleton<Security.IPasswordSecurer, Security.Password256HashSecurer>();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = exception
                    .Adapt<ErrorResponse>()
                    .WithCode(ErrorCodes.S_UnknownServerError);
                await context.Response.WriteAsJsonAsync(response);
            }));

            TypeAdapterConfig.GlobalSettings.Scan(typeof(Startup).Assembly);
            FluentValidation.ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("en-US");
        }
    }
}
