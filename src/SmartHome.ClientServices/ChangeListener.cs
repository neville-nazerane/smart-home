﻿using Microsoft.AspNetCore.SignalR.Client;
using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ClientServices
{
    public class ChangeListener
    {

        private readonly HubConnection _connection;

        public event EventHandler<DeviceChangedArgs> OnDeviceChanged;
        public event EventHandler<SceneChangedArgs> OnSceneChanged;

        public ChangeListener(string baseUrl)
        {
            _connection = new HubConnectionBuilder()
                                            .WithUrl($"{baseUrl}/hubs/changeNotify")
                                            .Build();

        }

        public async Task StartAsync()
        {
            SetupListeners();
            try
            {
                await _connection.StartAsync();
            }
            catch 
            {
                await Console.Out.WriteLineAsync("Failed to start signalR");
            }
        }

        void SetupListeners()
        {
            _connection.On<ListenedDevice>("deviceChanged", 
                                                d => OnDeviceChanged?.Invoke(this, new()
                                                {
                                                    Info = d
                                                }));

            _connection.On<Scene>("sceneChanged",
                                            s => OnSceneChanged?.Invoke(this, new()
                                            {
                                                Scene = s
                                            }));

        }

        public class DeviceChangedArgs : EventArgs
        {
            public ListenedDevice Info { get; set; }
        }

        public class SceneChangedArgs : EventArgs
        {
            public Scene Scene { get; set; }
        }

    }
}
