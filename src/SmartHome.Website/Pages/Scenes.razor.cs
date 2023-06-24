using Microsoft.AspNetCore.Components;
using SmartHome.ClientServices;
using SmartHome.Models;
using SmartHome.Website.Utilities;

namespace SmartHome.Website.Pages
{
    public partial class Scenes : IDisposable
    {
        private List<Scene> scenes;

        [Inject]
        public SmartContext SmartContext { get; set; }

        [Inject]
        public ChangeListener ChangeListener { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ChangeListener.OnSceneChanged += SceneChanged;
            scenes = await SmartContext.Scenes.GetAllScenesAsync().ToThatTypeAsync();
        }

        Task SwitchSceneAsync(Scene scene)
            => SmartContext.Scenes.SetSceneEnabledAsync(Enum.Parse<SceneName>(scene.Name), !scene.Enabled);

        private void SceneChanged(object sender, ChangeListener.SceneChangedArgs e)
        {
            var index = scenes.FindIndex(s => s.Name == e.Scene.Name);
            Console.WriteLine(e.Scene.Name + index);
            if (index > -1)
            {
                scenes[index] = e.Scene;
                StateHasChanged();
            }
        }

        public void Dispose()
        {
            InnerDispose();
            GC.SuppressFinalize(this);
        }

        ~Scenes()
        {
            InnerDispose();
        }

        void InnerDispose()
        {
            ChangeListener.OnSceneChanged -= SceneChanged;
        }

    }
}
