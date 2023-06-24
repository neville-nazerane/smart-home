using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Models.Contracts;
using SmartHome.ServerServices.Automation;
using SmartHome.ServerServices.Clients;
using SmartHome.ServerServices.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddAllServer(this IServiceCollection services, IConfiguration configuration)
        {

            var hueConfig = configuration.GetSection("hue");
            services.AddHttpClient<IPhilipsHueClient, PhilipsHueClient>(
                            c => PhilipsHueClient.SetupClient(c, hueConfig["baseUrl"], hueConfig["key"]))
                    .ConfigurePrimaryHttpMessageHandler(PhilipsHueClient.CreateHandler);

            var bondConfig = configuration.GetSection("bond");
            services.AddHttpClient<IBondClient, BondClient>(
                        c => BondClient.SetupClient(c, bondConfig["baseUrl"], bondConfig["token"]));

            var smartConfgs = configuration.GetSection("smartthings");
            services.AddHttpClient<ISmartThingsClient, SmartThingsClient>(
                        c => SmartThingsClient.SetupClient(c, smartConfgs["PAT"]));

            services.AddScoped<SmartContext>()
                    .AddScoped<IScenesService, ScenesService>()
                    .AddTransient<AutomationService>();

            var dbFile = $"{configuration["global:dataPath"]}/data.db";
            services.AddDbContext<AppDbContext>(c => c.UseSqlite($"Data Source={dbFile}",
                                                o => o.MigrationsAssembly("SmartHome.ServerServices")));

            return services;
        }

        public static async Task InitServicesAsync(this IServiceProvider serviceProvider)
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var sp = scope.ServiceProvider;

            var dbContext = sp.GetService<AppDbContext>();
            await dbContext.Database.MigrateAsync();
            await dbContext.SeedScenesAsync();
        }

    }
}
