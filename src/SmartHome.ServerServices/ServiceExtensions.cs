using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Models.ClientContracts;
using SmartHome.ServerServices.Clients;
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
            services.AddHttpClient<IPhilipsHueClient, PhilipsHueClient>(c => PhilipsHueClient.SetupClient(c, hueConfig["baseUrl"], hueConfig["key"]))
                    .ConfigurePrimaryHttpMessageHandler(PhilipsHueClient.GetHandler);

            services.AddTransient<SmartContext>();

            return services;
        }

    }
}
