using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Logging;

namespace FileMonitorService
{
    public class FilesWatcher
    {
        private string _path;
        private ILogger<Worker> _logger;

        public FilesWatcher(string path, ILogger<Worker> logger)
        {
            _path = path;
            _logger = logger;
        }
        public void WatchFiles()
        {
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = _path;

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                // Only watch text files.
                watcher.Filter = "*.txt";

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;
            }
        }
        private void OnChanged(object source, FileSystemEventArgs e) =>
        _logger.LogInformation($"File: {e.FullPath} {e.ChangeType}");

        private void OnRenamed(object source, RenamedEventArgs e) =>
        _logger.LogInformation($"File: {e.FullPath} {e.ChangeType}");
    }
}
