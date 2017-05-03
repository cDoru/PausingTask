using System.Threading.Tasks;

namespace Demo.Contract
{
    public interface ILongRunningComponent
    {
        Task RunAsync();
    }
}
