using System;

namespace Lucky38.UnitReact.Core
{
    public class WhereObservable<T> : ISubscribable<T>
    {
        private readonly ISubscribable<T> _source;
        private readonly Func<T, bool> _predicate;

        public WhereObservable(ISubscribable<T> source, Func<T, bool> predicate)
        {
            _source = source;
            _predicate = predicate;
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return _source.Subscribe(data =>
            {
                if (_predicate(data))
                    action(data);
            });
        }
    }
}
