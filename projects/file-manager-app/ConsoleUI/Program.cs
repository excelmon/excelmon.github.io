using AppLibrary.Helpers;

Console.WriteLine("Greetings!");
bool runNow = true;

if (runNow) 
{ 
    FileOrganizer.MoveOldFilesInPDrivePDFDirectory();
    FileOrganizer.OrganizeSuccessfullyMerged();
}

// The below methods are used for testing purposes. They can be uncommented to perform file renaming
// and moving operations on local test directories. Make sure to adjust the test directories and file
// types as needed before running these methods.
//FileOrganizer.RenameLocalTestFiles();
//FileOrganizer.MoveLocalTestFiles();


// This method is used to find any open files in the P Drive PDF Directory.It can be used to identify
// files that are currently being accessed and may need to be closed before performing file operations
// on them.
//FileOrganizer.FindOpenFilesInPDrivePDFDirectory();

Console.WriteLine("Job done!");
