using SmartHome.BackgroundProcessor;
using SmartHome.BackgroundProcessor.Services;
using SmartHome.ServerServices;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configs = hostContext.Configuration;
        services.AddAllServer(configs)
                .AddTransient<HueProcessor>()
                .AddTransient<BondProcessor>()
                .AddHttpClient<ApiConsumer>(c =>
                {
                    c.BaseAddress = new(configs["smarthomeEndpoint"]);
                });
        
        services
                .AddHostedService<HueListener>()
                .AddHostedService<BondListener>();
    })
    .Build();

await host.RunAsync();
