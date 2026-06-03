using AppLibrary.Helpers;
using AppLibrary.Loggers;
using System.Diagnostics;
using AppLibrary.Enums;

namespace AppLibrary.FileMovers
{
    public class FileMoveManager : IFileMoveManager, IDisposable
    {
        private readonly ILogger _logger;
        public string FileType { get; set; }
        public DirectoryInfo OriginDirectory { get; set; }
        public string DestinationPath { get; set; }

        public FileMoveManager(ILogger logger, string originPath, string destinationPath, string fileType)
        {
            _logger = logger;
            OriginDirectory = new DirectoryInfo(originPath);
            DestinationPath = destinationPath;
            FileType = fileType;
        }

        // Default MoveFiles (moves all files of a certain type)
        public void MoveFiles()
        {
            MoveFiles(searchWord: null, SubDirectoryType.None);
        }

        public void MoveFiles(string? searchWord) 
        {
            MoveFiles(searchWord, SubDirectoryType.None);
        }

        // Overloaded method that accepts an optional search word
        public void MoveFiles(string? searchWord, SubDirectoryType subDirectoryType)
        {
            try
            {
                string searchPattern = string.IsNullOrWhiteSpace(searchWord)
                    ? "*" + FileType
                    : "*" + searchWord + "*" + FileType;
                FileInfo[] filesInDirectory = OriginDirectory.GetFiles(searchPattern);

                string headerMessage = $"Moving '{FileType}' files {(searchWord != null ? $"containing '{searchWord}'" : "without filter")}.";
                _logger.WriteLog(headerMessage);
                var count = 0;

                Stopwatch stopwatch = new Stopwatch();
                foreach (FileInfo file in filesInDirectory)
                {
                    stopwatch.Restart();
                    // DestinationPath sorting options
                    string finalDestinationPath = subDirectoryType switch
                    {
                        SubDirectoryType.Year => DirectoryManager.DirectoryAsModifiedYear(DestinationPath, file),
                        SubDirectoryType.YearMonth => DirectoryManager.DirectoryAsModifiedYearThenMonth(DestinationPath, file),
                        _ => DestinationPath
                    };
                    DirectoryManager.ConfirmDestinationExists(finalDestinationPath);

                    bool fileLocked = FileUtilities.IsFileLocked(file);
                    if (fileLocked)
                    {
                        string fileDetails = FileUtilities.GetLockedFileDetails(file);
                        _logger.WriteLog(fileDetails);
                    }

                    try
                    {
                        // Getting the modified Year and Month should be a separate class DirectorySorter, Namer, 
                        file.MoveTo(finalDestinationPath + "\\" + file.Name);
                        count++;
                        stopwatch.Stop();
                        string fileMessage = $"file moved: {file.Name} | no: {count} | sec: {stopwatch.Elapsed.TotalSeconds}";
                        _logger.WriteLog(fileMessage);
                    }
                    catch (Exception e)
                    {
                        _logger.WriteLog($"failed file: {file.Name}");
                        _logger.WriteLog($"    exception: {e.Message}");
                    }
                }
            }
            finally 
            {
                Dispose();
            }
        }

        public void Dispose() => _logger.Dispose();
    }
}
