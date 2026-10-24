using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab5
{
    class Program
    {
        static Queue<Customer> queue = new Queue<Customer>();
        static object queueLock = new object();
        static int customerCounter = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Модель касс супермаркета");
            Console.WriteLine("Нажмите любую клавишу для остановки.\n");
            using var cts = new CancellationTokenSource();
            var token = cts.Token;
            var checkout1 = new Checkout(1, queue);
            var checkout2 = new Checkout(2, queue);
            var checkout3 = new Checkout(3, queue);
            var t1 = new Thread(() => checkout1.ProcessCustomers(token));
            var t2 = new Thread(() => checkout2.ProcessCustomers(token));
            var t3 = new Thread(() => checkout3.ProcessCustomers(token));
            t1.Start();
            t2.Start();
            t3.Start();
            var generatorThread = new Thread(() =>
            {
                Random rnd = new Random();
                while (!token.IsCancellationRequested)
                {
                    token.WaitHandle.WaitOne(rnd.Next(300, 500));
                    lock (queueLock)
                    {
                        queue.Enqueue(new Customer(++customerCounter));
                    }
                }
            });
            generatorThread.IsBackground = true;
            generatorThread.Start();
            var statsThread = new Thread(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    token.WaitHandle.WaitOne(2000);
                    if (token.IsCancellationRequested) break;

                    lock (queueLock)
                    {
                        Console.WriteLine($"[Статистика] В очереди: {queue.Count} чел. | " +
                                          $"Касса 1: {checkout1.GetServedCount()} | " +
                                          $"Касса 2: {checkout2.GetServedCount()} | " +
                                          $"Касса 3: {checkout3.GetServedCount()}");
                    }
                }
            });
            statsThread.IsBackground = true;
            statsThread.Start();
            Console.ReadKey();
            Console.WriteLine("\nОстановка программы...");
            cts.Cancel();
            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine("Все кассы успешно закрыты. Программа завершена.");
        }
    }
}