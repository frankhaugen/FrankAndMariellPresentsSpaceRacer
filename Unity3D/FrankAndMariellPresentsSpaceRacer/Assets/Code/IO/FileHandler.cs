using System.IO;
using System.Text;
using System.Threading;

namespace Code.IO
{
    internal static class FileHandler
    {
        private static ReaderWriterLock _locker = new ReaderWriterLock();

        public static void WriteString(string value, string path)
        {
            try
            {
                _locker.AcquireWriterLock(int.MaxValue);
                File.WriteAllText(path, value);
            }
            finally
            {
                _locker.ReleaseWriterLock();
            }
        }
        
        public static string ReadString(string path)
        {
            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 0x1000, FileOptions.SequentialScan);
            using var reader = new StreamReader(fileStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }    
}