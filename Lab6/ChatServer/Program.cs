using System.Net;
using System.Net.Sockets;

int Port = args.Length > 0 ? int.Parse(args[0]) : 5555;

var clients = new List<StreamWriter>();
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
    var endpoint = client.Client.RemoteEndPoint;
    var stream = client.GetStream();
    var reader = new StreamReader(stream);
    var writer = new StreamWriter(stream) { AutoFlush = true };

    string? nick = await reader.ReadLineAsync(); // первое сообщение — ник
    if (string.IsNullOrWhiteSpace(nick)) { client.Close(); return; }

    lock (lockObj) clients.Add(writer);
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} подключился ({endpoint})");
    await BroadcastAsync($"*** {nick} вошёл в чат ***");

    try
    {
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (line == "/exit") break;
            await BroadcastAsync($"[{DateTime.Now:HH:mm:ss}] {nick}: {line}");
        }
    }
    catch (IOException) { /* клиент оборвал соединение */ }
    finally
    {
        lock (lockObj) clients.Remove(writer);
        client.Close();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} отключился");
        await BroadcastAsync($"*** {nick} покинул чат ***");
    }
}

async Task BroadcastAsync(string message)
{
    List<StreamWriter> snapshot;
    lock (lockObj) snapshot = clients.ToList();  // копия под блокировкой

    foreach (var w in snapshot)
    {
        try { await w.WriteLineAsync(message); }
        catch (IOException) { /* отвалившийся клиент удалится в своей задаче */ }
    }
}