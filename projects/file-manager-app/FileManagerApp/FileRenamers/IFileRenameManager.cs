using AppLibrary.Enums;
using AppLibrary.GeneralInterface;

namespace AppLibrary.FileRenamers
{
    public interface IFileRenameManager : IDirectoryLocation, IDisposable
    {
        string FindWord { get; set; }
        string RenameTo { get; set; }
        RenameMode RenameMode { get; set; }
        void RenameFiles();
    }
}
