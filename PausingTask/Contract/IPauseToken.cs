using System.Threading.Tasks;

namespace PausingTask.Contract
{
    public interface IPauseToken
    {
        bool IsPaused { get; }
        Task WaitWhilePausedAsync();
        Task WaitWhilePausedWithResponseAsyc();
    }
}