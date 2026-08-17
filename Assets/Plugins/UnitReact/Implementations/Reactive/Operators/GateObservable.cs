using System;

namespace Lucky38.UnitReact.Core
{
    public class GateObservable<T> : ISubscribable<T>
    {
        private readonly ISubscribable<T> _source;
        private readonly float _cooldownSeconds;
        private readonly bool _skipIfRunning;

        public GateObservable(ISubscribable<T> source, float cooldownSeconds, bool skipIfRunning)
        {
            _source = source;
            _cooldownSeconds = cooldownSeconds;
            _skipIfRunning = skipIfRunning;
        }

        public IDisposable Subscribe(Action<T> action)
        {
            var gate = new SyncEventGate(_cooldownSeconds, _skipIfRunning);
            return _source.Subscribe(data =>
            {
                if (gate.TryEnter())
                {
                    try
                    {
                        action(data);
                    }
                    finally
                    {
                        gate.Exit();
                    }
                }
            });
        }
    }
}