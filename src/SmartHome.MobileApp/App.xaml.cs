using SmartHome.ClientServices;
using SmartHome.MobileApp.Pages;
using SmartHome.MobileApp.Utils;
using SmartHome.Models;

namespace SmartHome.MobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        public static void HandleAppActions(AppAction appAction)
        {
            Current.Dispatcher.Dispatch(async () =>
            {
                var context = MauiApplication.Current.Services.GetService<SmartContext>();
                var action = appAction.GetAction();
                await Current.MainPage.Navigation.PushAsync(new ActionRunningPage(action.GetDisplayname()));

                switch (action)
                {
                    case Models.AppActionType.TurnGoodNightOff:
                        await context.Scenes.SetSceneEnabledAsync(SceneName.GoodNight, false);
                        break;
                }
                
                Current.Quit();
            });
        }
    }
}