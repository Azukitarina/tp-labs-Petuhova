using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab5
{
    public class Customer
    {
        public int Id { get; }
        public Customer(int id) => Id = id;
    }
    public class Checkout
    {
        public int Number { get; }
        public int ServedCount { get; private set; }
        private readonly Queue<Customer> _queue;
        private readonly object _lockObj = new object();
        public Checkout(int number, Queue<Customer> queue)
        {
            Number = number;
            _queue = queue;
        }
        public void ProcessCustomers(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Customer? customer = null;
                lock (_lockObj)
                {
                    if (_queue.Count > 0)
                    {
                        customer = _queue.Dequeue();
                    }
                }
                if (customer != null)
                {
                    token.WaitHandle.WaitOne(new Random().Next(100, 1000));

                    lock (_lockObj)
                    {
                        ServedCount++;
                    }
                }
                else
                {
                    token.WaitHandle.WaitOne(100);
                }
            }
        }
        public int GetServedCount()
        {
            lock (_lockObj) { return ServedCount; }
        }
    }
}