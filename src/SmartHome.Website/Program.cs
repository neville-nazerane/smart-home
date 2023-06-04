using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartHome.ClientServices;
using SmartHome.Website;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string baseUrl = builder.Configuration["baseUrl"];

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["baseUrl"]) });

builder.Services.AddScoped(sp => new SmartContext(new HttpClient
{
    BaseAddress = new(baseUrl)
}));
builder.Services.AddSingleton(sp => new ChangeListener(baseUrl));

var app = builder.Build();

await app.Services.GetService<ChangeListener>().StartAsync();


await app.RunAsync();
