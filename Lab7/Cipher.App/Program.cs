using System;
using System.Text;
using Cipher.Core;

Console.OutputEncoding = Encoding.UTF8;
var fileStorage = new FileStorage("cipher_results.txt");
var consoleStorage = new ConsoleStorage();

while (true)
{
    Console.WriteLine("\nШифратор");
    Console.WriteLine("1 - Шифр Цезаря");
    Console.WriteLine("2 - Шифр Виженера");
    Console.WriteLine("0 - Выход");
    Console.Write("Выбор: ");

    string choice = Console.ReadLine();
    if (choice == "0") break;

    ICipher cipher = choice switch
    {
        "1" => new CaesarCipher(),
        "2" => new VigenereCipher(),
        _ => null
    };

    if (cipher == null) { Console.WriteLine("Неверный выбор."); continue; }

    Console.Write("Введите текст: ");
    string text = Console.ReadLine();

    Console.Write("Введите ключ (число для Цезаря, слово для Виженера): ");
    string key = Console.ReadLine();

    Console.Write("1 - Зашифровать, 2 - Расшифровать: ");
    string action = Console.ReadLine();

    try
    {
        string result = action == "1" ? cipher.Encrypt(text, key) : cipher.Decrypt(text, key);
        consoleStorage.Save(result);
        fileStorage.Save(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
}