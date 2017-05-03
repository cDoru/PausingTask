using System.Threading.Tasks;

namespace PausingTask.Contract
{
    public interface IPauseTokenSource
    {
        void Pause();
        void Resume();
        Task WaitWhilePausedAsync();
        Task PauseWithResponseAsync();
        Task WaitWhilePausedWithResponseAsyc();
        bool IsPaused { get; }
        IPauseToken Token { get; }
    }
}