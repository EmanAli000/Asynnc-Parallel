using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            #region Parallel Extensions Versus Sequential

            int[] arr = new int[1000];

            int counter = 0;

            var parallelStopWatch = new System.Diagnostics.Stopwatch();
            parallelStopWatch.Start();
            Parallel.ForEach(arr, item => {  Console.Write($"{counter++}, "); });
            parallelStopWatch.Stop();
            Console.WriteLine();
            Console.WriteLine($"Parallel - time to log {arr.Length} items = {parallelStopWatch.ElapsedMilliseconds} ms");

            counter = 0;

            var sequentialStopWatch = new System.Diagnostics.Stopwatch();
            sequentialStopWatch.Start();
            for(var i = 0; i< arr.Length; i++)
            {
                Console.Write($"{counter++}, ");
            }
            sequentialStopWatch.Stop();
            Console.WriteLine();
            Console.WriteLine($"sequential - time to log {arr.Length} items = {sequentialStopWatch.ElapsedMilliseconds} ms");

            //Parallel - time to log 1000 items = 768 ms
            //sequential time to log 1000 items = 82 ms
            //conclusion: sequential is faster because of the overhead of threads

            counter = 0;

            parallelStopWatch = new System.Diagnostics.Stopwatch();
            parallelStopWatch.Start();
            Parallel.ForEach(arr, new ParallelOptions() { MaxDegreeOfParallelism = 1 }, item => { Console.Write($"{counter++}, "); });
            parallelStopWatch.Stop();
            Console.WriteLine();
            Console.WriteLine($"Parallel - MaxDegreeOfParallelism = 1 - time to log {arr.Length} items = {parallelStopWatch.ElapsedMilliseconds} ms");

            //Parallel - MaxDegreeOfParallelism = 1 - time to log 1000 items = 66 ms
            //conclusion: time taken still greater than sequential

            #endregion

            #region Creating Threads Using Task.Run

            var threadsStopWatch = new System.Diagnostics.Stopwatch();
            threadsStopWatch.Start();

            var task1 = Task.Run(async () => {
                await Task.Delay(1000);
            });

            var task2 = Task.Run(async () => {
                await Task.Delay(10000);
            });

            var taskList = new List<Task>() { task1, task2 };
            
            Task.WhenAll(taskList).ContinueWith((i) => Console.WriteLine($"whenAll {threadsStopWatch.ElapsedMilliseconds} ms"));
            Task.WhenAny(taskList).ContinueWith((i) => Console.WriteLine($"whenAny {threadsStopWatch.ElapsedMilliseconds} ms"));

            //whenAny 1061 ms
            //whenAll 10052 ms

            Console.ReadLine();
            threadsStopWatch.Stop();

            #endregion

        }
    }
}
