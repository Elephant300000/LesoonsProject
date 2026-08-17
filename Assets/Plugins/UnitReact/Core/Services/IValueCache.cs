using System;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Provides a per-event-type value cache used for change detection
    /// and dispatch deduplication.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of value associated with an event type.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// This interface defines a lightweight caching mechanism that stores
    /// the last known value for each event type and determines whether
    /// a new value represents a meaningful change.
    /// </para>
    ///
    /// <para>
    /// Typical use cases include:
    /// <list type="bullet">
    /// <item><description>Preventing redundant event dispatches</description></item>
    /// <item><description>State-change driven reactive systems</description></item>
    /// <item><description>Optimizing high-frequency update loops</description></item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// The cache is keyed by event <see cref="Type"/>, allowing a single
    /// instance to track values for multiple event categories.
    /// </para>
    ///
    /// <para>
    /// Implementations may use custom equality logic to determine whether
    /// a value has changed.
    /// </para>
    ///
    /// <para>
    /// This interface does not prescribe any threading guarantees.
    /// Thread-safety must be handled by the implementation or the caller.
    /// </para>
    /// </remarks>
    public interface IValueCache<TValue>
    {
        /// <summary>
        /// Determines whether the value associated with the specified event type has changed.
        /// </summary>
        /// <param name="eventType">
        /// The event type used as a key in the cache.
        /// </param>
        /// <param name="newValue">
        /// The new value to compare against the cached value.
        /// </param>
        /// <returns>
        /// <c>true</c> if the value is different from the previously cached value
        /// or if no value was cached yet; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// If the value has changed, the cache is updated with <paramref name="newValue"/>.
        /// <para/>
        /// This method is typically used as a guard before invoking an event:
        /// <code>
        /// if (cache.HasChanged(typeof(MyEvent), value))
        ///     subject.InvokeAction(new MyEvent(value));
        /// </code>
        /// </remarks>
        bool HasChanged(Type eventType, TValue newValue);
        /// <summary>
        /// Gets the cached value for the specified event type.
        /// </summary>
        /// <param name="eventType">The event type key.</param>
        /// <returns>
        /// The cached value if present; otherwise, the default value of <typeparamref name="TValue"/>.
        /// </returns>
        /// <remarks>
        /// This method does not modify the cache state.
        /// </remarks>
        TValue Get(Type eventType);
        /// <summary>
        /// Clears the cached value associated with the specified event type.
        /// </summary>
        /// <param name="eventType">The event type key to remove.</param>
        /// <remarks>
        /// After calling this method, the next <see cref="HasChanged"/> call
        /// for the same event type will return <c>true</c>.
        /// </remarks>
        void Clear(Type eventType);
    }
}