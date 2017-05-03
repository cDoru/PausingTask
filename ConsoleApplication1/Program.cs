using System;
using System.Threading;
using System.Threading.Tasks;
using PausingTask.Contract;
using PausingTask.Implementation;

namespace Demo
{
    class Program
    {
        static PauseTokenSource GetPauseTokenSource()
        {
            return new PauseTokenSource();
        }

        static void Main(string[] args)
        {
            var pausetoken = GetPauseTokenSource();
            Console.WriteLine("starting task");

            new Task(()=> RunningTask(pausetoken.Token)).Start();

            Console.WriteLine("task started");

            Thread.Sleep(5000);
            pausetoken.Pause();
            Console.WriteLine("sleeping 5s");
            Thread.Sleep(5000);
            pausetoken.Resume();
            Thread.Sleep(5000);
            pausetoken.Pause();
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        private static async Task RunningTask(IPauseToken token)
        {
            var i = 0;
            Thread.Sleep(1000);
            while (true)
            {
                Console.WriteLine(i++);
                await Task.Delay(10);
                await token.WaitWhilePausedAsync();
            }
        }
    } 
}
