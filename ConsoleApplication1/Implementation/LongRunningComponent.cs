using System;
using System.Threading.Tasks;
using Demo.Contract;
using PausingTask.Contract;

namespace Demo.Implementation
{
    public class LongRunningComponent : ILongRunningComponent
    {
        private readonly IPauseToken _pauseToken;

        public LongRunningComponent(Func<IPauseToken> pauseToken)
        {
            _pauseToken = pauseToken();
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.WriteLine("sleeping");
                await Task.Delay(1000);
                await _pauseToken.WaitWhilePausedAsync();
                Console.WriteLine("Is Paused = {0}", _pauseToken.IsPaused);
            }
        }
    }
}