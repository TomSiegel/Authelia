using Microsoft.EntityFrameworkCore;
using Authelia.Server.Configuration;
using Authelia.Database.Model;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;

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

            TypeAdapterConfig.GlobalSettings.Scan(typeof(Startup).Assembly);
            FluentValidation.ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("en-US");
        }
    }
}
