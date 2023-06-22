using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices
{
    public class ScenesService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly AppDbContext _dbContext;
        private readonly ISignalRPusher _signalRPusher;
        SmartContext context;
        SmartContext Context => context ??= _serviceProvider.GetService<SmartContext>();

        SmartDevices Devices => Context.Devices;

        public ScenesService(IServiceProvider serviceProvider,
                             AppDbContext dbContext,
                             ISignalRPusher signalRPusher)
        {
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
            _signalRPusher = signalRPusher;
        }

        public Task<bool> IsEnabledAsync(SceneName sceneName, CancellationToken cancellationToken = default) 
            => _dbContext.Scenes
                         .AsNoTracking()
                         .Where(s => s.Name == sceneName.ToString())
                         .Select(s => s.Enabled)
                         .SingleAsync(cancellationToken);

        public async Task SetSceneEnabled(SceneName sceneName, bool isEnabled, CancellationToken cancellationToken = default)
        {
            var scene =  await _dbContext.Scenes
                                         .SingleAsync(s => s.Name == sceneName.ToString(), cancellationToken);
            scene.Enabled = isEnabled;
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            await _signalRPusher.NotifySceneChangeAsync(scene, cancellationToken);
        }
    }
}
