using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

int Port = args.Length > 0 ? int.Parse(args[0]) : 5555;

var clients = new Dictionary<string, StreamWriter>();
var lockObj = new object();

var listener = new TcpListener(IPAddress.Any, Port);
listener.Start();
Console.WriteLine($"Сервер запущен на порту {Port}. Ctrl+C — остановка.");

while (true)
{
    TcpClient client = await listener.AcceptTcpClientAsync();
    _ = HandleClientAsync(client);              // каждого клиента — в свою задачу
}

async Task HandleClientAsync(TcpClient client)
{
    var stream = client.GetStream();
    var reader = new StreamReader(stream);
    var writer = new StreamWriter(stream) { AutoFlush = true };

    string? nick = null; // первое сообщение — ник
    try
    {
        while (true)
        {
            nick = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(nick)) { client.Close(); return; }
            lock (lockObj)
            {
                if (clients.ContainsKey(nick))
                {
                    _ = writer.WriteLineAsync("ERROR: Ник занят. Введите другой.");
                    continue;
                }
                clients.Add(nick, writer);
            }
            break;
        }
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} подключился");
        await BroadcastAsync($"*** {nick} вошёл в чат ***");
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (line == "/exit") break;
            if (line == "/list")
            {
                lock (lockObj)
                {
                    string list = string.Join(", ", clients.Keys);
                    _ = writer.WriteLineAsync($"Пользователи онлайн: {list}");
                }
                continue;
            }
            await BroadcastAsync($"[{DateTime.Now:HH:mm:ss}] {nick}: {line}");
        }
    }
    catch (IOException) { /* клиент оборвал соединение */ }
    catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
    finally
    {
        if (nick != null)
        { 
            lock (lockObj) clients.Remove(nick);
            client.Close();
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} отключился");
            await BroadcastAsync($"*** {nick} покинул чат ***");
        } 
    }
}

async Task BroadcastAsync(string message)
{
    List<StreamWriter> snapshot;
    lock (lockObj) snapshot = clients.Values.ToList();  // копия под блокировкой

    foreach (var w in snapshot)
    {
        try { await w.WriteLineAsync(message); }
        catch (IOException) { /* отвалившийся клиент удалится в своей задаче */ }
    }
}