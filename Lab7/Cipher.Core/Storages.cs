using System;
using System.IO;

namespace Cipher.Core
{
    public class ConsoleStorage : IResultStorage
    {
        public void Save(string result) => Console.WriteLine($"Результат: {result}");
    }
    public class FileStorage : IResultStorage
    {
        private readonly string _filePath;
        public FileStorage(string filePath) => _filePath = filePath;
        public void Save(string result) => File.AppendAllText(_filePath, $"{DateTime.Now}: {result}\n");
    }

}
