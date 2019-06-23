using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;

namespace ChatServer
{
    public class MemoryMappedTextFile : IDisposable
    {
        private readonly MemoryMappedFile _file;
        private readonly Mutex _mutex;

        public MemoryMappedTextFile(string fileName, int capacityInBytes)
        {
            _file = MemoryMappedFile.CreateOrOpen(fileName, capacityInBytes);
            _mutex = new Mutex(false, $"{fileName}Mutex", out bool createdNew);
        }

        public string ReadAllText()
        {
            _mutex.WaitOne();

            string content = GetFileContent();

            _mutex.ReleaseMutex();

            return content;
        }

        public void AppendText(string text)
        {
            _mutex.WaitOne();

            WriteToFile(GetFileContent() + text);

            _mutex.ReleaseMutex();
        }

        private string GetFileContent()
        {
            using (MemoryMappedViewStream stream = _file.CreateViewStream())
            using (var reader = new BinaryReader(stream))
            {
                return reader.ReadAllUTF8Text();
            }
        }

        private void WriteToFile(string text)
        {
            using (MemoryMappedViewStream stream = _file.CreateViewStream(0, 0))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(Encoding.UTF8.GetBytes(text));
            }
        }

        public void Dispose()
        {
            _file.Dispose();
            _mutex.Dispose();
        }
    }
}
