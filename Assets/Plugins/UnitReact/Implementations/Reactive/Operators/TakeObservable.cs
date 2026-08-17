using System;

namespace Lucky38.UnitReact.Core
{
    public class TakeObservable<T> : ISubscribable<T>
    {
        private readonly ISubscribable<T> _source;
        private readonly int _count;

        public TakeObservable(ISubscribable<T> source, int count)
        {
            _source = source;
            _count = count;
        }

        public IDisposable Subscribe(Action<T> action)
            => TakeSubscription<T>.Create(_source, _count, action);
    }
}
