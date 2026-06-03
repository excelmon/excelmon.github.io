namespace AppLibrary.GeneralInterface
{
    public interface IDirectoryLocation
    {
        DirectoryInfo OriginDirectory { get; set; }
        string FileType { get; set; }
    }
}