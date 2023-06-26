using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices.Scenes
{
    public partial class ScenesService : IScenesService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ScenesService> _logger;
        private readonly ISignalRPusher _signalRPusher;

        SmartContext context;
        SmartContext Context => context ??= _serviceProvider.GetService<SmartContext>();

        SmartDevices Devices => Context.Devices;

        public ScenesService(IServiceProvider serviceProvider,
                             AppDbContext dbContext,
                             ILogger<ScenesService> logger,
                             ISignalRPusher signalRPusher)
        {
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
            _logger = logger;
            _signalRPusher = signalRPusher;
        }

        public async Task SwitchAsync(SceneName sceneName, CancellationToken cancellationToken = default)
        {
            await SetSceneEnabledAsync(sceneName,
                                       !await IsEnabledAsync(sceneName, cancellationToken),
                                       cancellationToken);
        }

        public Task<bool> IsEnabledAsync(SceneName sceneName, CancellationToken cancellationToken = default)
            => _dbContext.Scenes
                         .AsNoTracking()
                         .Where(s => s.Name == sceneName.ToString())
                         .Select(s => s.Enabled)
                         .SingleAsync(cancellationToken);

        public Task<bool> IsAnySceneEnabledAsync(params SceneName[] sceneNames)
            => _dbContext.Scenes
                         .AsNoTracking()
                         .Where(s => s.Enabled && sceneNames.Select(sc => sc.ToString()).Contains(s.Name))
                         .AnyAsync();

        public async Task SetSceneEnabledAsync(SceneName sceneName, bool isEnabled, CancellationToken cancellationToken = default)
        {
            var scene = await _dbContext.Scenes
                                         .SingleAsync(s => s.Name == sceneName.ToString(), cancellationToken);
            if (isEnabled == scene.Enabled) return;
            scene.Enabled = isEnabled;
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _signalRPusher.NotifySceneChangeAsync(scene, cancellationToken);
            await OnSceneChangedAsync(sceneName);
        }

        public async Task<IEnumerable<Scene>> GetAllScenesAsync(CancellationToken cancellationToken = default)
            => await _dbContext.Scenes.ToListAsync(cancellationToken);
    }
}
