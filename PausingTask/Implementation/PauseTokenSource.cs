using System;
using System.Threading;
using System.Threading.Tasks;
using PausingTask.Contract;

namespace PausingTask.Implementation
{
    public class PauseTokenSource : IPauseTokenSource
    {
        private static readonly Task CompletedTask = Task.FromResult(true);
        private readonly object _lockObject = new Object();
        private bool _paused; // could use _resumeRequest as flag too
        private TaskCompletionSource<bool> _pauseResponse;
        private TaskCompletionSource<bool> _resumeRequest;

        public void Pause()
        {
            lock (_lockObject)
            {
                if (_paused)
                    return;
                _paused = true;
                _pauseResponse = null;
                _resumeRequest = new TaskCompletionSource<bool>();
            }
        }

        public void Resume()
        {
            TaskCompletionSource<bool> resumeRequest;

            lock (_lockObject)
            {
                if (!_paused)
                    return;
                _paused = false;
                resumeRequest = _resumeRequest;
                _resumeRequest = null;
            }

            resumeRequest.TrySetResult(true);
        }

        public Task WaitWhilePausedAsync()
        {
            lock (_lockObject)
            {
                return !_paused ? CompletedTask : _resumeRequest.Task;
            }
        }

        // pause with feedback
        // that the producer task has reached the paused state

        public Task PauseWithResponseAsync()
        {
            Task responseTask;

            lock (_lockObject)
            {
                if (_paused)
                    return _pauseResponse.Task;
                _paused = true;
                _pauseResponse = new TaskCompletionSource<bool>();
                _resumeRequest = new TaskCompletionSource<bool>();
                responseTask = _pauseResponse.Task;
            }

            return responseTask;
        }

        public Task WaitWhilePausedWithResponseAsyc()
        {
            Task resumeTask;
            TaskCompletionSource<bool> response;

            lock (_lockObject)
            {
                if (!_paused)
                    return CompletedTask;
                response = _pauseResponse;
                resumeTask = _resumeRequest.Task;
            }

            if (response != null)
            {
                response.TrySetResult(true);
            }

            return resumeTask;
        }

        public bool IsPaused
        {
            get
            {
                lock (_lockObject)
                    return _paused;
            }
        }

        public IPauseToken Token
        {
            get { return new PauseToken(this); }
        }
    }
}