namespace Lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
            Console.WriteLine("Данные успешно приняты. Нажмите Enter для выхода");
            Console.ReadLine();
        }
    }
}
