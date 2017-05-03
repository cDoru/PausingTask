using System.Threading.Tasks;
using PausingTask.Contract;

namespace PausingTask.Implementation
{
    public struct PauseToken : IPauseToken
    {
        private readonly IPauseTokenSource _source;

        public PauseToken(IPauseTokenSource source)
        {
            _source = source;
        }

        public bool IsPaused
        {
            get { return _source != null && _source.IsPaused; }
        }

        public Task WaitWhilePausedAsync()
        {
            return _source.WaitWhilePausedAsync();
        }

        public Task WaitWhilePausedWithResponseAsyc()
        {
            return _source.WaitWhilePausedWithResponseAsyc();
        }
    }
}
