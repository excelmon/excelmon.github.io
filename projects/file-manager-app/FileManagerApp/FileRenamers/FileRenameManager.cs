using AppLibrary.Enums;
using AppLibrary.Loggers;

namespace AppLibrary.FileRenamers
{
    public class FileRenameManager : IFileRenameManager, IDisposable
    {
        private readonly ILogger _logger;
        public string FileType { get; set; }
        public DirectoryInfo OriginDirectory { get; set; }
        public string FindWord { get; set; }
        public string RenameTo { get; set; }
        public RenameMode RenameMode { get; set; }
        public FileRenameManager(ILogger logger, string originPath, string findWord, string renameTo, string fileType, RenameMode renameMode) 
        {
            _logger = logger;
            OriginDirectory = new DirectoryInfo(originPath);
            FindWord = findWord;
            RenameTo = renameTo;
            FileType = fileType;
            RenameMode = renameMode;
        }
        public string GetFullPath() => OriginDirectory.FullName;

        public void RenameFiles() 
        {
            try
            {
                string searchPattern = RenameMode == RenameMode.ByExtension
                    ? "*" + FileType
                    : "*" + FindWord + "*" + FileType;

                FileInfo[] filesInDirectory = OriginDirectory.GetFiles(searchPattern);

                string headerMessage = RenameMode == RenameMode.ByExtension
                    ? $"Renaming extensions '{FindWord}' to '{RenameTo}'."
                    : $"Renaming '{FileType}' files with text '{FindWord}' to '{RenameTo}'.";

                _logger.WriteLog(headerMessage);

                foreach (FileInfo file in filesInDirectory)
                {
                    string originalName = file.Name;
                    string newFileName = RenameMode == RenameMode.ByExtension
                        ? Path.GetFileNameWithoutExtension(file.FullName) + RenameTo
                        : file.Name.Replace(FindWord, RenameTo);

                    try
                    {
                        file.MoveTo(Path.Combine(OriginDirectory.FullName, newFileName));
                        string fileMessage = $"renamed {originalName} to {newFileName}";
                        _logger.WriteLog(fileMessage);
                    }
                    catch (Exception e)
                    {
                        _logger.WriteLog($"failed rename file: {file.Name}");
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
