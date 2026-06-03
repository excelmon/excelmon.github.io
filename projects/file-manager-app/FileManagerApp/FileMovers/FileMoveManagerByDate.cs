using AppLibrary.Helpers;
using AppLibrary.Loggers;
using System.Diagnostics;
using AppLibrary.Enums;

namespace AppLibrary.FileMovers
{
    public class FileMoveManagerByDate : IFileMoveManagerByDate
    {
        private readonly ILogger _logger;
        public string FileType { get; set; }
        public DirectoryInfo OriginDirectory { get; set; }
        public string DestinationPath { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }

        public FileMoveManagerByDate(ILogger logger, string originPath, string destinationPath, string fileType, DateOnly fromDate, DateOnly toDate)
        {
            _logger = logger;
            OriginDirectory = new DirectoryInfo(originPath);
            DestinationPath = destinationPath;
            FileType = fileType;
            FromDate = fromDate;
            ToDate = toDate;
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

        /// <summary>
        /// Overloaded method that accepts an optional search word
        /// </summary>
        /// <param name="searchWord"></param>
        /// <param name="subDirectoryMove">Moves file to a subfolder by last modified date</param>
        /// <param name="subDirectoryType">Y = Year//, YM = Year//Month// </param>
        public void MoveFiles(string? searchWord, SubDirectoryType subDirectoryType)
        {
            try
            {
                string searchPattern = string.IsNullOrWhiteSpace(searchWord)
                    ? "*" + FileType
                    : "*" + searchWord + "*" + FileType;
                FileInfo[] filesInDirectory = OriginDirectory.GetFiles(searchPattern);
                string headerMessage = $"Moving '{FileType}' files from: {FromDate} to: {ToDate} {(searchWord != null ? $"containing '{searchWord}'" : "without filter")}.";
                _logger.WriteLog(headerMessage);
                var count = 0;
                Stopwatch stopwatch = new Stopwatch();
                foreach (FileInfo file in filesInDirectory)
                {
                    DateOnly thisFileModifiedDate = file.LastWriteTime.ToDateOnly();
                    if (thisFileModifiedDate >= FromDate && thisFileModifiedDate <= ToDate)
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
                            file.MoveTo(finalDestinationPath + "\\" + file.Name);
                            count++;
                            stopwatch.Stop();
                            string fileMessage = $"file moved: {file.Name} | {thisFileModifiedDate} | no: {count} | sec: {stopwatch.Elapsed.TotalSeconds}";
                            _logger.WriteLog(fileMessage);
                        }
                        catch (Exception e)
                        {
                            _logger.WriteLog($"failed file: {file.Name}");
                            _logger.WriteLog($"    exception: {e.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"file: {file.Name} | {thisFileModifiedDate} is outside of range");
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
