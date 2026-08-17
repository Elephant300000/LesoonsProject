using System.Threading;

namespace Lucky38.UnitReact.Core
{
    internal class HubAsyncGate : SyncEventGate
    {
        private CancellationTokenSource _cts;

        public HubAsyncGate(float cooldownSeconds, bool skipIfRunning) 
            : base(cooldownSeconds, skipIfRunning) { }

        public bool TryEnter(out CancellationToken token)
        {
            if (!base.TryEnter())
            {
                token = CancellationToken.None;
                return false;
            }

            _cts = new CancellationTokenSource();
            token = _cts.Token;
            return true;
        }

        public void Complete()
        {
            _cts?.Dispose();
            _cts = null;
            base.Exit();
        }
    }
}