namespace AppLibrary.FileMovers
{
    public interface IFileMoveManagerByDate : IFileMoveManager, IDisposable
    {
        DateOnly FromDate { get; set; }
        DateOnly ToDate { get; set; }
    }
}
