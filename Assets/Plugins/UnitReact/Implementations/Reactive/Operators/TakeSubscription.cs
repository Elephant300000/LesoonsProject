using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Lucky38.UnitReact.Core
{
    internal sealed class TakeSubscription<T> : IDisposable
    {
        private static readonly ConcurrentBag<TakeSubscription<T>> _pool = new();

        private int _remaining;
        private Action<T> _action;
        private IDisposable _sourceSubscription;
        private int _isDisposed;

        private readonly Action<T> _onNextDelegate;

        private TakeSubscription()
        {
            _onNextDelegate = OnNext;
        }

        public static TakeSubscription<T> Create(ISubscribable<T> source, int count, Action<T> action)
        {
            if (!_pool.TryTake(out var sub)) sub = new TakeSubscription<T>();

            sub._remaining = count;
            sub._action = action;
            sub._isDisposed = 0; 
            sub._sourceSubscription = source.Subscribe(sub._onNextDelegate);

            return sub;
        }

        private void OnNext(T data)
        {
            if (_isDisposed == 1 || _remaining <= 0) return;

            _remaining--;
            try
            {
                _action(data);
            }
            finally
            {
                // Даже если action кинул — Take обязан отписаться, иначе утечка.
                // Логирование исключения делает источник (Subject / Hub / Property).
                if (_remaining == 0)
                    Dispose();
            }
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _isDisposed, 1) == 1) return;

            _sourceSubscription?.Dispose();
            _sourceSubscription = null;
            _action = null;
            _pool.Add(this);  
        }
    }
}