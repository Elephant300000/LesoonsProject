using System;

namespace Lucky38.UnitReact.Core
{
    public class SkipObservable<T> : ISubscribable<T>
    {
        private readonly ISubscribable<T> _source;
        private readonly int _count;

        public SkipObservable(ISubscribable<T> source, int count)
        {
            _source = source;
            _count = count;
        }

        public IDisposable Subscribe(Action<T> action)
        {
            int remaining = _count;
            return _source.Subscribe(data =>
            {
                if (remaining > 0)
                {
                    remaining--;
                    return;
                }

                action(data);
            });
        }
    }
}
