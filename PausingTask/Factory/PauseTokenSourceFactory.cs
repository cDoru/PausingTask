using Autofac;
using PausingTask.Contract;

namespace PausingTask.Factory
{
    public interface IPauseTokenSourceFactory
    {
        IPauseTokenSource CreateNewPauseTokenSource();
    }

    public class PauseTokenSourceFactory : IPauseTokenSourceFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public PauseTokenSourceFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public IPauseTokenSource CreateNewPauseTokenSource()
        {
            return _lifetimeScope.Resolve<IPauseTokenSource>();
        }
    }
}
