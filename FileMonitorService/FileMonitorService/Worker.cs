using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileMonitorService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private FilesWatcher _filesWatcher;
        private const string PATH = @"C:\DarTemp\FilesToListen";

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
                try
                {
                    _filesWatcher.WatchFiles();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "There was an ERROR");
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
