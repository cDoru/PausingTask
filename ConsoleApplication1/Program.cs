using System;
using System.Threading;
using System.Threading.Tasks;
using PausingTask;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var pausetoken = new PauseTokenSource();
            Console.WriteLine("starting task");
            var task = new Task(() => RunningTask(pausetoken.Token), TaskCreationOptions.LongRunning);
            task.Start();
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

        private static async void RunningTask(PauseToken token)
        {
            while (true)
            {
                Console.WriteLine("sleeping");
                await Task.Delay(1000);
                await token.WaitWhilePausedAsync();
                Console.WriteLine("Is Paused = {0}", token.IsPaused);
            }
        }

    }
}
