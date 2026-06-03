
using AppLibrary.GeneralInterface;

namespace AppLibrary.FileMovers
{
    public interface IFileOpenManager: IDirectoryLocation, IDisposable
    {
        void FindOpenFiles();
        void FindOpenFiles(string? searchWord);
    }
}