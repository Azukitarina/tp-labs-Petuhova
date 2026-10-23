using System.Net.Sockets;
using System;
using System.IO;
Console.Write("Адрес сервера (Enter = localhost): ");
string host = Console.ReadLine() is { Length: > 0 } h ? h : "localhost";

using var client = new TcpClient();
try
{
    await client.ConnectAsync(host, 5555);
}
catch (SocketException)
{
    Console.WriteLine("Не удалось подключиться к серверу. Проверьте, запущен ли он.");
    return;
}
var stream = client.GetStream();
var reader = new StreamReader(stream);
var writer = new StreamWriter(stream) { AutoFlush = true };
while (true)
{
    Console.Write("Ваш ник: ");
    string nick = Console.ReadLine() ?? "anon";
    await writer.WriteLineAsync(nick);               // представились серверу
    string? response = await reader.ReadLineAsync();
    if (response != null && response.StartsWith("ERROR"))
    {
        Console.WriteLine(response);
        continue;
    }
    Console.WriteLine("Подключено. Пишите сообщения, /exit — выход.");
    break;
}
_ = Task.Run(async () =>
{
    try
    {
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
            Console.WriteLine(line);
    }
    catch (IOException) { }
    Console.WriteLine("Соединение с сервером потеряно.");
});
while (true)
{
    string? msg = Console.ReadLine();
    if (msg == null) continue;
    await writer.WriteLineAsync(msg);
    if (msg == "/exit") break;
}