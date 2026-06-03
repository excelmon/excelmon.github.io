using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLibrary.Loggers
{
    public class Logger : ILogger
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        private readonly string _fullPath;
        private readonly StreamWriter _writer;
        private bool _disposed = false;

        public Logger(string filePath, string fileName)
        {
            FilePath = filePath;
            FileName = fileName;
            _fullPath = Path.Combine(FilePath, FileName);
            Directory.CreateDirectory(FilePath); // no-op if exists
            _writer = File.CreateText(_fullPath);
        }

        public void WriteLog(string message)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            Console.WriteLine(message);
            _writer.WriteLine($"{DateTime.Now}: {message}");
            _writer.Flush();
        }

        public void Close() => Dispose();

        public void Dispose()
        {
            if (!_disposed)
            {
                _writer?.Close();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
