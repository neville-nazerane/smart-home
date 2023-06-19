using Microsoft.Extensions.Configuration;
using SmartHome.Models;
using SmartHome.Models.ClientContracts;
using SmartHome.ServerServices;
using SmartHome.ServerServices.Clients;
using SmartHome.WebAPI;
using SmartHome.WebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
services.AddAllServer(configuration)
        .AddCors()
        .AddSignalR();


var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials());

app.MapGet("/", () => "Hello Smart Home");

app.MapHub<ChangeNotifyHub>("/hubs/changeNotify");

app.MapAllEndpoints();

await app.Services.InitServicesAsync();

await app.RunAsync();

