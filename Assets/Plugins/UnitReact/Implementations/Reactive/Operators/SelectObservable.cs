using System;

namespace Lucky38.UnitReact.Core
{

    internal class SelectObservable<TSource, TResult> : ISubscribable<TResult>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, TResult> _selector;

        internal SelectObservable(ISubscribable<TSource> source, Func<TSource, TResult> selector)
        {
            _source = source;
            _selector = selector;
        }

        public IDisposable Subscribe(Action<TResult> action)
        {
            return _source.Subscribe(data =>
            {
                action(_selector(data));
            });
        }
    }
}