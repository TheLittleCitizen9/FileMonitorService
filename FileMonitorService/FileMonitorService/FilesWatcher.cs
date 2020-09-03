using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace FileMonitorService
{
    public class FilesWatcher
    {
        private string _path;
        private ILogger<Worker> _logger;
        private IConfigurationRoot _configuration;

        public FilesWatcher(ILogger<Worker> logger)
        {
            CreateConfiguration();
            _path = _configuration.GetSection("Path").Value;
            _logger = logger;
        }
        public void WatchFiles()
        {
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = _path;

                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName;


                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                watcher.EnableRaisingEvents = true;

                while (true) ;
            }
        }
        private void OnChanged(object source, FileSystemEventArgs e) =>
        _logger.LogInformation($"File {e.ChangeType}: {e.FullPath}");

        private void OnRenamed(object source, RenamedEventArgs e) =>
        _logger.LogInformation($"File {e.ChangeType}: {e.OldFullPath} -> {e.FullPath}");

        private void CreateConfiguration()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        }
    }
}
