using System;
using System.Threading.Tasks;

namespace PausingTask
{
    public class PauseTokenSource
    {
        private readonly object _mLockObject = new Object();
        private bool _mPaused; // could use m_resumeRequest as flag too

        internal static readonly Task s_completedTask = Task.FromResult(true);
        private TaskCompletionSource<bool> m_pauseResponse;
        private TaskCompletionSource<bool> m_resumeRequest;

        public void Pause()
        {
            lock (_mLockObject)
            {
                if (_mPaused)
                    return;
                _mPaused = true;
                m_pauseResponse = null;
                m_resumeRequest = new TaskCompletionSource<bool>();
            }
        }

        public void Resume()
        {
            TaskCompletionSource<bool> resumeRequest;

            lock (_mLockObject)
            {
                if (!_mPaused)
                    return;
                _mPaused = false;
                resumeRequest = m_resumeRequest;
                m_resumeRequest = null;
            }

            resumeRequest.TrySetResult(true);
        }

        public Task WaitWhilePausedAsync()
        {
            lock (_mLockObject)
            {
                return !_mPaused ? s_completedTask : m_resumeRequest.Task;
            }
        }

        // pause with feedback
        // that the producer task has reached the paused state

        public Task PauseWithResponseAsync()
        {
            Task responseTask;

            lock (_mLockObject)
            {
                if (_mPaused)
                    return m_pauseResponse.Task;
                _mPaused = true;
                m_pauseResponse = new TaskCompletionSource<bool>();
                m_resumeRequest = new TaskCompletionSource<bool>();
                responseTask = m_pauseResponse.Task;
            }

            return responseTask;
        }

        public Task WaitWhilePausedWithResponseAsyc()
        {
            Task resumeTask;
            TaskCompletionSource<bool> response;

            lock (_mLockObject)
            {
                if (!_mPaused)
                    return s_completedTask;
                response = m_pauseResponse;
                resumeTask = m_resumeRequest.Task;
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
                lock (_mLockObject)
                    return _mPaused;
            }
        }

        public PauseToken Token
        {
            get { return new PauseToken(this); }
        }
    }
}