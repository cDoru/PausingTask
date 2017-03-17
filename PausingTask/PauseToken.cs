using System.Threading.Tasks;

namespace PausingTask
{
    public struct PauseToken
    {
        private readonly PauseTokenSource _mSource;

        public PauseToken(PauseTokenSource source)
        {
            _mSource = source;
        }

        public bool IsPaused
        {
            get { return _mSource != null && _mSource.IsPaused; }
        }

        public Task WaitWhilePausedAsync()
        {
            return IsPaused
                ? _mSource.WaitWhilePausedAsync()
                : PauseTokenSource.s_completedTask;
        }

        public Task WaitWhilePausedWithResponseAsyc()
        {
            return IsPaused
                ? _mSource.WaitWhilePausedWithResponseAsyc()
                : PauseTokenSource.s_completedTask;
        }
    }
}
