using Cysharp.Threading.Tasks;
using System;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Defines an asynchronous event subject that uses pooled event instances.
    /// </summary>
    /// <remarks>
    /// This interface extends <see cref="IAsyncEventSubject"/> by adding support
    /// for event pooling via <see cref="IPoolableEvent"/>.
    ///
    /// The subject is responsible for:
    /// - Renting event instances from the pool
    /// - Passing them to observers
    /// - Returning them back to the pool after invocation
    ///
    /// This minimizes allocations and is intended for high-frequency events.
    /// </remarks>
    public interface IPooledAsyncEventSubject: IAsyncEventSubject
    {
        /// <summary>
        /// Invokes an asynchronous pooled action event.
        /// </summary>
        UniTask InvokePooledActionAsync<TContainer>(Action<TContainer> fill)
            where TContainer : class, IPoolableEvent, new();
    }
}
