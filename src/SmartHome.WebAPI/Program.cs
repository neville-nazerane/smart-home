
using Microsoft.Extensions.Configuration;
using SmartHome.Models;
using SmartHome.Models.ClientContracts;
using SmartHome.ServerServices;
using SmartHome.ServerServices.Clients;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;


var hueConfig = configuration.GetSection("hue");
services.AddHttpClient<IPhilipsHueClient, PhilipsHueClient>(c => PhilipsHueClient.SetupClient(c, hueConfig["baseUrl"], hueConfig["key"]))
        .ConfigurePrimaryHttpMessageHandler(PhilipsHueClient.GetHandler);

services.AddTransient<SmartContext>();

var app = builder.Build();

app.MapGet("/devices", DevicesAsync);
app.MapGet("/", () => "Hello Web World!");

await app.RunAsync();

static Task<IEnumerable<DeviceModelBase>> DevicesAsync(SmartContext context) => context.FetchAllDevicesAsync();

