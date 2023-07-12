using SmartHome.ClientServices;
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

                switch (appAction.GetAction())
                {
                    case Models.AppActionType.TurnGoodNightOff:
                        var context = MauiApplication.Current.Services.GetService<SmartContext>();
                        var isEnabled = await context.Scenes.IsEnabledAsync(SceneName.GoodNight);
                        await context.Scenes.SetSceneEnabledAsync(SceneName.GoodNight, !isEnabled);
                        break;
                }

                Current.Quit();

            });
        }
    }
}