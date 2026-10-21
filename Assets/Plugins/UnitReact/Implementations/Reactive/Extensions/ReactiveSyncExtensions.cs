using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
    public static class ReactiveSyncExtensions
    {
        public static ISubscribable<TResult> Select<TSource, TResult>(this ISubscribable<TSource> source, Func<TSource, TResult> selector)
        {
            return new SelectObservable<TSource, TResult>(source, selector);
        }

        public static ISubscribable<T> DistinctUntilChanged<T>(this ISubscribable<T> source, IEqualityComparer<T> comparer = null)
        {
            return new DistinctUntilChangedObservable<T, T>(source, x => x, comparer);
        }

        public static ISubscribable<T> DistinctUntilChanged<T, TKey>(this ISubscribable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer = null)
        {
            return new DistinctUntilChangedObservable<T, TKey>(source, keySelector, comparer);
        }


        public static WhereObservable<T> Where<T>(this ISubscribable<T> source, Func<T, bool> predicate)
        {
            return new WhereObservable<T>(source, predicate);
        }

        public static TakeObservable<T> Take<T>(this ISubscribable<T> source, int count)
                                                     => new TakeObservable<T>(source, count);

        public static SkipObservable<T> Skip<T>(this ISubscribable<T> source, int count)
        {
            return new SkipObservable<T>(source, count);
        }

        public static GateObservable<T> Gate<T>(this ISubscribable<T> source, float cooldownSeconds, bool skipIfRunning = true)
        {
            return new GateObservable<T>(source, cooldownSeconds, skipIfRunning);
        }

        public static IDisposable Subscribe(this ISubscribable<Unit> source, Action action)
        {
            return source.Subscribe(_ => action());
        }

        public static void AddTo(this IDisposable disposable, CompositeDisposable manager)
            => manager.Add(disposable);
    }
}