using System;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Defines the ability to handle synchronous events.
    /// </summary>
    /// <remarks>
    /// This interface represents the execution phase of an observer
    /// for synchronous event dispatch.
    ///
    /// Methods defined here are invoked exclusively by a subject
    /// and should never be called directly by user code.
    /// </remarks>
    public interface ISyncObserverHandler : IObserver
    {
        /// <summary>
        /// Invokes a synchronous action handler for the specified event.
        /// </summary>
        /// <typeparam name="TContainer">The event type.</typeparam>
        /// <param name="container">The event instance.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no compatible handler is registered.
        /// </exception>
        void React<TContainer>(TContainer evt) where TContainer : class;
    }
}