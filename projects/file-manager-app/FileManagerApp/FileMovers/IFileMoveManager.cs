using AppLibrary.GeneralInterface;
using AppLibrary.Enums;

namespace AppLibrary.FileMovers
{
    public interface IFileMoveManager : IDirectoryLocation, IDisposable
    {
        string DestinationPath { get; set; }
        void MoveFiles();
        void MoveFiles(string? searchWord);
        void MoveFiles(string? searchWord, SubDirectoryType subDirectoryType);
    }
}
