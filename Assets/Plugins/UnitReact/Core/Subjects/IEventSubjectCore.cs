using System;
using System.Collections.Generic;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Core contract for an event subject.
    /// </summary>
    /// <remarks>
    /// This interface represents the minimal infrastructure required
    /// to support the Subject side of the Subject�Observer pattern.
    ///
    /// Responsibilities:
    /// - Managing observer subscriptions
    /// - Cleaning up dead (garbage-collected) observers
    /// - Validating invocation signatures
    /// - Producing stable snapshots of active observers
    ///
    /// This interface does not define how events are invoked
    /// (synchronous, asynchronous, pooled, etc.).
    /// Invocation semantics are defined by higher-level subject interfaces.
    /// </remarks>
    public interface IEventSubjectCore
    {
        /// <summary>
        /// Subscribes an observer to the specified event type.
        /// </summary>
        /// <param name="eventType">The event type to subscribe to.</param>
        /// <param name="observer">The observer context.</param>
        /// <remarks>
        /// <para>
        /// Observers are stored using weak references to avoid memory leaks.
        /// </para>
        /// <para>
        /// Dead observers are automatically cleaned up.
        /// Duplicate subscriptions are ignored.
        /// </para>
        /// </remarks>
        void Subscribe(Type eventType, IObserver observer);
        /// <summary>
        /// Unsubscribes an observer from the specified event type.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <param name="observer">The observer context.</param>
        void Unsubscribe(Type eventType, IObserver observer);

        /// <summary>
        /// Creates a snapshot of currently alive observers for the given event type.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <returns>
        /// A pooled list containing strong references to alive observers.
        /// </returns>
        /// <remarks>
        /// Dead observers are removed during snapshot creation.
        /// The returned list must be returned to <see cref="ListPool{T}"/>.
        /// </remarks>
        List<ISyncObserverHandler> SnapshotSyncObserver(Type t);
        List<IAsyncObserverHandler> SnapshotAsyncObserver(Type t);
    }
}
