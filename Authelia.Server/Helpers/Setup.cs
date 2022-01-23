using Microsoft.EntityFrameworkCore;
using Authelia.Server.Configuration;
using Authelia.Server.Extensions;
using Authelia.Database.Model;
using FluentValidation.AspNetCore;
using Mapster;

namespace Authelia.Server.Helpers
{
    public class Setup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Setup>(scan =>
                {
                    return true;
                }, ServiceLifetime.Singleton);
            });
            services.AddDbContext<AutheliaDbContext>(options =>
            {
                var settings = configuration.GetSection("DbConnection").Get<DbConnectionOptions>();

                options.UseMySQL(settings.GetConnectionString());
            });
            services.AddSingleton<Security.IPasswordSecurer, Security.Password256HashSecurer>();
            services.AddAutheliaAuthentication();
            services.AddAutheliaAuthorization();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
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
