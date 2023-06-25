﻿using Microsoft.Extensions.Logging;
using SmartHome.ClientServices;
using SmartHome.MobileApp.Pages;
using SmartHome.MobileApp.ViewModels;
using System.Net.Http;

namespace SmartHome.MobileApp
{
    public static class MauiProgram
    {

        static readonly HttpClient httpClient = new();

        public static MauiApp CreateMauiApp()
        {


            httpClient.BaseAddress = new("http://192.168.1.155:5010");


            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var services = builder.Services;
            services.AddTransient(c => new SmartContext(httpClient))

                    .AddTransient<ScenesPage>()
                    .AddTransient<ScenesViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}