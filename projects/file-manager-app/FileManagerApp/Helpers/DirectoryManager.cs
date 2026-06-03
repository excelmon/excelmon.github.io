namespace AppLibrary.Helpers
{
    /// <summary>
    /// Methods for folder creation (check if exists) and folder naming conventions
    /// </summary>
    public static class DirectoryManager
    {
        // PDF Attachment Folders
        public static readonly string PDriveBaseDirectory = @"P:\YourNetworkShare\YourFolder";
        public static readonly string PDrivePDFDirectory = Path.Combine(PDriveBaseDirectory, "PDF Attachments\\To Be Sweeped\\");
        public static readonly string PDriveSuccessfullyMerged = Path.Combine(PDriveBaseDirectory, "PDF Attachments\\Successfully Merged\\");
        public static readonly string PDriveOlderThan2Weeks = Path.Combine(PDriveBaseDirectory, "PDF Attachments\\Older Than 2 Weeks\\");
        public static readonly string PDriveOlderThan2WeeksLogger = Path.Combine(PDriveOlderThan2Weeks, "File Movement Log\\");

        // Child Support Folder
        public static readonly string PDriveInboundChildSupportFiles = Path.Combine(PDriveBaseDirectory, "Child Support\\Inbound\\");

        // Local Test Folders + Logger Folders
        public static readonly string LocalBaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static readonly string LocalLogger = Path.Combine(LocalBaseDirectory, "Documents\\Logger\\");
        public static readonly string LocalTestOrigin = Path.Combine(LocalBaseDirectory, "Documents\\Test File Move - Origin\\");
        public static readonly string LocalTestDestination = Path.Combine(LocalBaseDirectory, "Documents\\Test File Move - Dest\\");

        public static void ConfirmDestinationExists(string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }
        }

        /// <summary>
        /// <para>Year // {monthNumberString} - {monthString} // File</para>
        /// <para>2025 // 3 - March // myfile.pdf </para>
        /// </summary>
        /// <param name="destinationFolder">Parent folder path</param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string DirectoryAsModifiedYearThenMonth(string destinationFolder, FileInfo file) 
        {
            DateOnly thisFileModifiedDate = file.LastWriteTime.ToDateOnly();
            string monthString = thisFileModifiedDate.ToString("MMMM");
            string yearString = thisFileModifiedDate.Year.ToString();
            int monthNumber = thisFileModifiedDate.Month;
            string monthNumberString = monthNumber.ToString(); // Convert to string if needed
            string monthNumberNameCombo = $"{monthNumberString} - {monthString}";
            string destinationSubFolder = Path.Combine(destinationFolder, yearString + "\\" + monthNumberNameCombo);
            return destinationSubFolder;
        }

        /// <summary>
        /// <para>Year // File</para>
        /// <para>2025 // myfile.pdf</para>
        /// </summary>
        /// <param name="destinationFolder">Parent folder path</param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string DirectoryAsModifiedYear(string destinationFolder, FileInfo file)
        {
            DateOnly thisFileModifiedDate = file.LastWriteTime.ToDateOnly();
            string yearString = thisFileModifiedDate.Year.ToString();
            string destinationSubFolder = Path.Combine(destinationFolder, yearString);
            return destinationSubFolder;
        }
    }
}
