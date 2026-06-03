using AppLibrary.Helpers;
using AppLibrary.Loggers;

namespace AppLibrary.FileMovers
{
    internal class FileOpenManager : IFileOpenManager, IDisposable
    {
        private readonly ILogger _logger;
        public string FileType { get; set; }
        public DirectoryInfo OriginDirectory { get; set; }

        public FileOpenManager(ILogger logger, string originPath, string fileType)
        {
            _logger = logger;
            OriginDirectory = new DirectoryInfo(originPath);
            FileType = fileType;
        }

        public void FindOpenFiles()
        {
            FindOpenFiles(searchWord: null);
        }

        public void FindOpenFiles(string? searchWord)
        {
            try
            {
                string searchPattern = string.IsNullOrWhiteSpace(searchWord)
                    ? "*" + FileType
                    : "*" + searchWord + "*" + FileType;
                FileInfo[] filesInDirectory = OriginDirectory.GetFiles(searchPattern);
                var openCount = 0;
                string headerMessage = $"Checking for open '{FileType}' files at {OriginDirectory} {(searchWord != null ? $"containing '{searchWord}'" : "without filter")}.";
                _logger.WriteLog(headerMessage);
                foreach (FileInfo file in filesInDirectory)
                {
                    bool fileLocked = FileUtilities.IsFileLocked(file);
                    if (fileLocked)
                    {
                        string fileDetails = FileUtilities.GetLockedFileDetails(file);
                        _logger.WriteLog(fileDetails);
                        openCount += 1;
                    }
                    else
                    {
                        Console.WriteLine($"{file.Name} is not locked.");
                    }
                }
                _logger.WriteLog("Open file review completed.");
                _logger.WriteLog($"Total open files: {openCount}");
            }
            finally 
            {
                Dispose();
            }
        }

        public void Dispose() => _logger.Dispose();
    }
}
