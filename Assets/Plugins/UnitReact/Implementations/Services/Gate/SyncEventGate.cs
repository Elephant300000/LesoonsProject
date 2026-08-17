using System;
using System.Threading;

namespace Lucky38.UnitReact.Core
{
    internal class SyncEventGate : ISyncEventGate
    {
        public SyncEventGate(float cooldownSeconds, bool skipIfRunning)
        {
            _cooldownTicks = (long)(cooldownSeconds * TimeSpan.TicksPerSecond);
            _skipIfRunning = skipIfRunning;
        }

        protected readonly long _cooldownTicks;
        protected readonly bool _skipIfRunning;

        protected long _lastInvokeTicks;
        protected int _isExecuting; // 0 = free, 1 = busy

        public bool TryEnter()
        {
            if (_skipIfRunning && Interlocked.CompareExchange(ref _isExecuting, 1, 0) == 1)
                return false;

            if (_cooldownTicks > 0)
            {
                long now = DateTime.UtcNow.Ticks;
                long last = Interlocked.Read(ref _lastInvokeTicks);

                if (now - last < _cooldownTicks)
                {
                    if (_skipIfRunning)
                        Interlocked.Exchange(ref _isExecuting, 0);
                    return false;
                }

                Interlocked.Exchange(ref _lastInvokeTicks, now);
            }

            // Keep Enter/Exit pairing even when skipIfRunning is off.
            if (!_skipIfRunning)
                Interlocked.Exchange(ref _isExecuting, 1);

            return true;
        }

        public void Exit()
        {
            Interlocked.Exchange(ref _isExecuting, 0);
        }
    }
}
