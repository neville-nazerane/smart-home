using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.BackgroundProcessor.Services;

namespace SmartHome.BackgroundProcessor
{
    public class BondListener : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BondListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var processor = scope.ServiceProvider.GetService<BondProcessor>();

            await Task.WhenAll(
                    processor.KeepListeningAsync(stoppingToken),
                    processor.ProcessQueueAsync(stoppingToken)
            );
        }


        //async Task StartListeningAsync(string baseAddress, CancellationToken cancellationToken = default)
        //{

        //}

    }
}
