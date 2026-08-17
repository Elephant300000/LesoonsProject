using System;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Defines a synchronous event subject that uses pooled event instances.
    /// </summary>
    /// <remarks>
    /// This interface extends <see cref="ISyncEventSubject"/> by adding support
    /// for event pooling via <see cref="IPoolableEvent"/>.
    ///
    /// It is designed for performance-critical synchronous event dispatch
    /// where allocations must be minimized.
    /// </remarks>
    public interface IPooledSyncEventSubject: ISyncEventSubject
    {
        /// <summary>
        /// Invokes a pooled action event.
        /// </summary>
        void InvokePooledAction<TContainer>(Action<TContainer> fill) where TContainer : class, IPoolableEvent, new();
    }
}
