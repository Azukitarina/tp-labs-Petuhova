namespace Lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            static void WriteDirectoryTree(string path, int maxDepth, StreamWriter writer, string indent = "", int currentDepth = 0)
            {
                if (currentDepth > maxDepth) return;
                string[] direcories = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);
                int totalItems = direcories.Length + files.Length;
                int currentItem = 0;
                foreach (string dir in direcories)
                {
                    currentItem++;
                    bool isLast = currentItem == totalItems;
                    string prefix = isLast ? "└──" : "├──";
                    writer.WriteLine($"{indent}{prefix}{Path.GetFileName(dir)}/");
                    string childIndent = indent + (isLast ? "    " : "│   ");
                    WriteDirectoryTree(dir, maxDepth, writer, childIndent, currentDepth + 1);
                }
                foreach (string file in files)
                {
                    currentItem++;
                    bool isLast = currentItem == totalItems;
                    string prefix = isLast ? "└── " : "├── ";
                    FileInfo info = new FileInfo(file);
                    writer.WriteLine($"{indent}{prefix}{info.Name} ({info.Length} байт)");
                }
            }
            Console.WriteLine("Генератор дерева каталога");
            Console.Write("Введите путь к папке: ");
            string folderPath = Console.ReadLine()!;
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Ошибка: Папка не существует.");
                return;
            }
            Console.Write("Введите максимальную глубину (0 - только корень): ");
            string depthInput = Console.ReadLine()!;
            if (!int.TryParse(depthInput, out int maxDepth) || maxDepth < 0)
            {
                Console.WriteLine("Ошибка: Глубина должна быть целым неотрицательным числом.");
                return;
            }
            string outFile = "tree.txt";
            using (StreamWriter writer = new StreamWriter(outFile))
            {
                writer.WriteLine($"Дерево каталога: {folderPath}");
                WriteDirectoryTree(folderPath, maxDepth, writer);
            }
            Console.WriteLine($"\nДерево успешно записано в файл: {Path.GetFullPath(outFile)}");
            Console.WriteLine("Данные успешно приняты. Нажмите Enter для выхода");
            Console.ReadLine();
        }
    }
}
