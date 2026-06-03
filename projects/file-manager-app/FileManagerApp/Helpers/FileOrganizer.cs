using AppLibrary.Enums;
using AppLibrary.FileMovers;
using AppLibrary.FileRenamers;

namespace AppLibrary.Helpers
{
    public static class FileOrganizer
    {
        public static void MoveOldFilesInPDrivePDFDirectory()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            using IFileMoveManagerByDate fileManager = Factory.CreateFileMoveManagerByDate(
                DirectoryManager.PDrivePDFDirectory, DirectoryManager.PDriveOlderThan2Weeks, ".pdf",
                fromDate: new DateOnly(2025, 1, 1), 
                toDate: today.AddDays(-14), 
                DirectoryManager.PDriveOlderThan2WeeksLogger, "app-log-nt-" + DateTimeExtends.TodayAsString() + ".txt");
            fileManager.MoveFiles();
        }

        public static void FindOpenFilesInPDrivePDFDirectory() 
        {
            using IFileOpenManager fileManager = Factory.CreateFileOpenManager(
                DirectoryManager.PDrivePDFDirectory, ".pdf",
                DirectoryManager.LocalLogger, "app-log-findOpen-" + DateTimeExtends.NowAsString() + ".txt");
            fileManager.FindOpenFiles();
        }

        public static void OrganizeSuccessfullyMerged() 
        {
            using IFileMoveManager fileManager = Factory.CreateFileMoveManager(
                DirectoryManager.PDriveSuccessfullyMerged, DirectoryManager.PDriveSuccessfullyMerged, ".pdf",
                DirectoryManager.LocalLogger, "app-log-sm-" + DateTimeExtends.TodayAsString() + ".txt");
            fileManager.MoveFiles(searchWord:null, SubDirectoryType.YearMonth);
        }

        public static void MoveLocalTestFiles() 
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            using IFileMoveManager fileManager = Factory.CreateFileMoveManager(
                DirectoryManager.LocalTestOrigin, DirectoryManager.LocalTestDestination, ".xlsx",
                DirectoryManager.LocalLogger, "app-log-move-" + DateTimeExtends.NowAsString() + ".txt");
            fileManager.MoveFiles("IWO");
        }

        public static void RenameLocalTestFiles() 
        {
            using IFileRenameManager fileManager = Factory.CreateFileRenameManager(
                DirectoryManager.LocalTestOrigin, DirectoryManager.LocalLogger,
                "app-log-rename-" + DateTimeExtends.NowAsString() + ".txt",
                "IWO", "Incoming Withholding Order", ".docx", RenameMode.ByText);
            fileManager.RenameFiles();
        }
    }
}
