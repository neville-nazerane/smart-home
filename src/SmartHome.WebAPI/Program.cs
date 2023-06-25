using Microsoft.Extensions.Configuration;
using SmartHome.Models;
using SmartHome.ServerServices;
using SmartHome.ServerServices.Clients;
using SmartHome.WebAPI;
using SmartHome.WebAPI.Hubs;
using SmartHome.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
services.AddAllServer(configuration)
        .AddTransient<ISignalRPusher, SignalRPusher>()
        .AddCors()
        .AddSignalR();


var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGet("/batman/{num}", RunMeAsync);

app.MapGet("/", () => "Hello Smart Home");

app.MapHub<ChangeNotifyHub>("/hubs/changeNotify");

app.MapAllEndpoints();

await app.Services.InitServicesAsync();

await app.RunAsync();

static Task RunMeAsync(SmartContext context, double num)
    => context.Devices.MiddleLight.SetBrightnessAsync(num);
