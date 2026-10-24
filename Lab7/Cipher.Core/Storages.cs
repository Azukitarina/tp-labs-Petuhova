using System;
using System.IO;

namespace Cipher.Core
{
    public class ConsoleStorage : IResultStorage
    {
        public void Save(string result) => ConsoleStorage.WriteLine($"Результат: {result}");
    }
    public class FileStorage : IResultStorage
    {
        private readonly string _filePath;
        public FileStorage(string filePath) => _filePath = filePath;
        public void Save(sring result) => FileStorage.AppendAllText(_filePath, $"{DateTime.Now}: {result}\n");
    }

}
