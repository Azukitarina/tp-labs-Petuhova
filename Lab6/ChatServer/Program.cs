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
List<string> history = new List<string>();
const string LogFile = "chat.log";

var listener = new TcpListener(IPAddress.Any, Port);
listener.Start();
Console.WriteLine($"Сервер запущен на порту {Port}. Ctrl+C — остановка.");

while (true)
{
    TcpClient client = await listener.AcceptTcpClientAsync();
    _ = HandleClientAsync(client);              // каждого клиента — в свою задачу
}
void LogToFile(string text)
{
    try
    {
        File.AppendAllText(LogFile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {text}\n");
    }
    catch { }
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
        LogToFile($"{nick} подключился");
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} подключился");
        lock (lockObj)
        {
            if (history.Count > 0)
            {
                _ = writer.WriteLineAsync("Последние сообщения");
                foreach (var msg in history)
                {
                    _ = writer.WriteLineAsync(msg);
                }
                _ = writer.WriteLineAsync(" ");
            }
        }
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
            if (line.StartsWith("/w "))
            {
                var parts = line.Split(' ', 3);
                if (parts.Length < 3)
                {
                    _ = writer.WriteLineAsync("Использование: /w ник текст");
                    continue;
                }
                string targetNick = parts[1];
                string msgText = parts[2];

                lock (lockObj)
                {
                    if (clients.TryGetValue(targetNick, out var targetWriter))
                    {
                        _ = targetWriter.WriteLineAsync($"[Личное от {nick}]: {msgText}");
                        _ = writer.WriteLineAsync($"[Вы -> {targetNick}]: {msgText}");
                        LogToFile($"{nick} -> {targetNick}: {msgText}");
                    }
                    else
                    {
                        _ = writer.WriteLineAsync($"Пользователь {targetNick} не найден.");
                    }
                }
                continue;
            }
            LogToFile($"{nick}: {line}");
            await BroadcastAsync($"[{DateTime.Now:HH:mm:ss}] {nick}: {line}");
        }
    }
    catch (IOException) { }
    catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
    finally
    {
        if (nick != null)
        { 
            lock (lockObj) clients.Remove(nick);
            client.Close();
            LogToFile($"{nick} отключился");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} отключился");
            await BroadcastAsync($"*** {nick} покинул чат ***");
        } 
    }
}

async Task BroadcastAsync(string message)
{
    List<StreamWriter> snapshot;
    lock (lockObj)
    {
        snapshot = clients.Values.ToList();
        history.Add(message);
        if (history.Count > 10) history.RemoveAt(0);
    }
    foreach (var w in snapshot)
    {
        try { await w.WriteLineAsync(message); }
        catch (IOException) { }
    }
}