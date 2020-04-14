using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;

namespace ThreadPoolQ2
{
    class Program
    {
        private CountdownEvent _countdown;
        private Random _random;
        private Int32 _threadsCount;
        private ConcurrentQueue<int> _integers; 

        public Program(int threadsCount)
        {
            _random = new Random();
            _integers = new ConcurrentQueue<int>();
            _threadsCount = threadsCount;
            _countdown = new CountdownEvent(threadsCount);
        }

        static void Main(string[] args)
        {
            var program = new Program(10);
            program.Run();

            Console.WriteLine(program._integers.Count);
            Console.WriteLine(program._integers.Sum());
            Console.ReadLine();
        }

        public void Run()
        {
            for (int i = 0; i < _threadsCount; i++)
            {
                ThreadPool.QueueUserWorkItem(x =>
                {
                    for(int j = 0; j < 100000; j++)
                    {
                        _integers.Enqueue(_random.Next(1, 100));
                    }
                    _countdown.Signal();
                });
            }

            while (_countdown.CurrentCount > 0)
            {
                Console.WriteLine("Waiting for threads to terminate ...");
                Thread.Sleep(100);                
            }
            _countdown.Wait();       

            Console.WriteLine("All threads finished");         
        }
    }
}
