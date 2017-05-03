﻿using System.Threading.Tasks;
using PausingTask.Contract;

namespace PausingTask.Implementation
{
    public struct PauseToken : IPauseToken
    {
        private readonly IPauseTokenSource _mSource;

        public PauseToken(IPauseTokenSource source)
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
                : PauseTokenSource.CompletedTask;
        }

        public Task WaitWhilePausedWithResponseAsyc()
        {
            return IsPaused
                ? _mSource.WaitWhilePausedWithResponseAsyc()
                : PauseTokenSource.CompletedTask;
        }
    }
}
