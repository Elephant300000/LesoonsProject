using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Lucky38.UnitReact.Core
{
    public class Subscription : IDisposable
    {
        // Используем ConcurrentBag для потокобезопасности пула
        private static readonly ConcurrentBag<Subscription> _pool = new();
        private Action _onDispose;
         
        private Subscription() { }

        public static Subscription Create(Action onDispose)
        {
            if (!_pool.TryTake(out var sub)) sub = new Subscription();
            sub._onDispose = onDispose;
            return sub;
        }

        public void Dispose()
        {
            var action = Interlocked.Exchange(ref _onDispose, null);
            if (action != null)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
                finally
                {
                    _pool.Add(this); // Возвращаем в пул
                }
            }
        }
    }
}