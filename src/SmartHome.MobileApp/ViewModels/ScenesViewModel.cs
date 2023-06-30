using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartHome.ClientServices;
using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.MobileApp.ViewModels
{
    public partial class ScenesViewModel : ObservableObject
    {

        [ObservableProperty]
        ObservableCollection<Scene> scenes;

        private readonly SmartContext _context;
        private readonly ChangeListener _changeListener;

        public ScenesViewModel(SmartContext context, ChangeListener changeListener)
        {
            _context = context;
            _changeListener = changeListener;
        }

        public void Subscribe()
        {
            _changeListener.OnSceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged(object sender, ChangeListener.SceneChangedArgs e)
        {
            
            int index = Scenes.ToList().FindIndex(s => s.Name == e.Scene.Name);
            if (index > -1)
                Scenes[index] = e.Scene;
        }

        [RelayCommand]
        async Task SwitchSceneAsync(Scene scene, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Scenes.SwitchAsync(Enum.Parse<SceneName>(scene.Name), cancellationToken);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task InitAsync()
        {
            await _changeListener.StartAsync();
            Scenes = new(await _context.Scenes.GetAllScenesAsync());
        }

        public void Unsubscribe()
        {
            _changeListener.OnSceneChanged -= OnSceneChanged;
        }

    }

}
