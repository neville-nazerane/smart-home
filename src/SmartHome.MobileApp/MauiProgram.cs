using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SmartHome.ClientServices;
using SmartHome.MobileApp.Pages;
using SmartHome.MobileApp.ViewModels;
using System.Net.Http;

namespace SmartHome.MobileApp
{
    public static class MauiProgram
    {
        private const string baseUrl = "http://192.168.1.155:5010";
        static readonly HttpClient httpClient = new();

        public static MauiApp CreateMauiApp()
        {

            httpClient.BaseAddress = new(baseUrl);


            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureEssentials(essentials =>
                {
                    essentials
                        .AddAppAction("scene", "Scene")
                        .AddAppAction("computer", "Computer Setup")
                        .OnAppAction(HandleAppActions);
                });

            var services = builder.Services;
            services
                    .AddTransient(c => new SmartContext(httpClient))
                    .AddScoped(p => new ChangeListener(baseUrl))

                    .AddTransient<ComputerPage>()
                    .AddTransient<ComputerViewModel>()

                    .AddTransient<ScenesPage>()
                    .AddTransient<ScenesViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }


        public static void HandleAppActions(AppAction appAction)
        {
            App.Current.Dispatcher.Dispatch(async () =>
            {

                switch (appAction.Id)
                {
                    case "scene":
                        await Shell.Current.GoToAsync("//scene");
                        break;
                    case "computer":
                        await Shell.Current.GoToAsync("//computer");
                        break;
                }
            });
        }
    }
}