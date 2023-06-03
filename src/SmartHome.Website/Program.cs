using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartHome.ClientServices;
using SmartHome.Website;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string url = builder.Configuration["baseUrl"];

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["baseUrl"]) });

builder.Services.AddScoped(sp => new SmartContext(new HttpClient
{
    BaseAddress = new(builder.Configuration["baseUrl"])
}));

await builder.Build().RunAsync();
