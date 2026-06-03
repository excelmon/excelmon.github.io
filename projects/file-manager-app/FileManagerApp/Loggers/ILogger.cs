namespace AppLibrary.Loggers
{
    public interface ILogger : IDisposable
    {
        string FilePath { get; }
        string FileName { get; }
        void WriteLog(string message);
        void Close();
    }
}