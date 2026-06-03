using AppLibrary.Enums;
using AppLibrary.FileMovers;
using AppLibrary.FileRenamers;
using AppLibrary.Loggers;

namespace AppLibrary.Helpers
{
    public static class Factory
    {
        public static IFileMoveManager CreateFileMoveManager(string originPath, string destinationPath, string fileType, string loggerFilePath, 
            string loggerFileName) 
        { 
            return new FileMoveManager(CreateLogger(loggerFilePath, loggerFileName), originPath, destinationPath, fileType);
        }

        /// <summary>
        /// Rename file in Same Directory
        /// </summary>
        /// <param name="originPath">Directory Path</param>
        /// <param name="loggerFilePath"></param>
        /// <param name="loggerFileName"></param>
        /// <param name="findWord">Key word to be renamed</param>
        /// <param name="renameTo">Replacement word</param>
        /// <param name="fileType">File extension for search. Not referenced if renaming a bad extension.</param>
        public static IFileRenameManager CreateFileRenameManager(string originPath, string loggerFilePath, string loggerFileName, 
            string findWord, string renameTo, string fileType, RenameMode renameMode)
        { 
            return new FileRenameManager(CreateLogger(loggerFilePath, loggerFileName), originPath, findWord, renameTo, fileType, renameMode);
        }

        public static IFileMoveManagerByDate CreateFileMoveManagerByDate(string originPath, string destinationPath, string fileType, 
            DateOnly fromDate, DateOnly toDate, string loggerFilePath, string loggerFileName)
        {
            return new FileMoveManagerByDate(CreateLogger(loggerFilePath, loggerFileName), originPath, destinationPath, fileType, fromDate, toDate);
        }

        public static IFileOpenManager CreateFileOpenManager(string originPath, string fileType, string loggerFilePath, string loggerFileName) 
        { 
            return new FileOpenManager(CreateLogger(loggerFilePath, loggerFileName), originPath, fileType);
        }

        public static ILogger CreateLogger(string filePath, string fileName) 
        { 
            return new Logger(filePath, fileName);
        }
    }
}
