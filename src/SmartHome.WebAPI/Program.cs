using Microsoft.Extensions.Configuration;
using SmartHome.Models;
using SmartHome.Models.ClientContracts;
using SmartHome.ServerServices;
using SmartHome.ServerServices.Clients;
using SmartHome.WebAPI;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;


var hueConfig = configuration.GetSection("hue");
services.AddHttpClient<IPhilipsHueClient, PhilipsHueClient>(c => PhilipsHueClient.SetupClient(c, hueConfig["baseUrl"], hueConfig["key"]))
        .ConfigurePrimaryHttpMessageHandler(PhilipsHueClient.GetHandler);

services.AddTransient<SmartContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello Smart Home");
app.MapAllEndpoints();

await app.RunAsync();


