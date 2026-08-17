using System;
using System.Threading;

namespace Lucky38.UnitReact.Core
{
    internal sealed class AsyncEventGate : SyncEventGate, IAsyncEventGate
    {
        private CancellationTokenSource _cts;
        private bool _disposed;

        public AsyncEventGate(float cooldownSeconds, bool skipIfRunning)
            : base(cooldownSeconds, skipIfRunning)
        {
            _cts = new CancellationTokenSource();
        }

        public bool TryEnter(out CancellationToken token)
        {
            if (_disposed || _cts == null)
            {
                token = CancellationToken.None;
                return false;
            }

            token = _cts.Token;
            if (token.IsCancellationRequested)
                return false;

            return base.TryEnter();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                if (_cts != null && !_cts.IsCancellationRequested)
                    _cts.Cancel();
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
                Exit();
            }
        }
    }
}
