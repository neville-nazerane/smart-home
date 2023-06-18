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
                .AddTransient<MainProcessor>()
                .AddSingleton<ListenerQueue>()
                .AddTransient<DbProcesses>()
                .AddHttpClient<ApiConsumer>(c =>
                {
                    c.BaseAddress = new(configs["smarthomeEndpoint"]);
                });
        
        services
                .AddHostedService<HueListener>()
                .AddHostedService<MainListener>()
                .AddHostedService<BondListener>();
    })
    .Build();

await host.Services.InitServicesAsync();

await host.RunAsync();
