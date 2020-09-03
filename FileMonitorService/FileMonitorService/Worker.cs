using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileMonitorService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private FilesWatcher _filesWatcher;
        private const string PATH = @"C:\temp\FilesToListen";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _filesWatcher = new FilesWatcher(PATH, _logger);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Service started at: {DateTimeOffset.Now}");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Service stopped at: {DateTimeOffset.Now}");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _filesWatcher.WatchFiles();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
