using System;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Defines the ability to register synchronous event handlers.
    /// </summary>
    /// <remarks>
    /// This interface represents the configuration phase of an observer.
    ///
    /// Implementations are responsible for:
    /// - Declaring which event types the observer is interested in
    /// - Registering the expected invocation signatures
    ///
    /// Registration does not automatically subscribe the observer;
    /// subscription is typically performed as a separate lifecycle step.
    /// </remarks>
    public interface ISyncObserverRegistrar : IObserver
    {
        /// <summary>
        /// Registers a synchronous action handler for the specified event type.
        /// </summary>
        /// <typeparam name="TContainer">The event type.</typeparam>
        /// <param name="handler">The action to invoke when the event is dispatched.</param>
        /// <remarks>
        /// This method:
        /// <list type="bullet">
        /// <item><description>Registers the expected signature in the signature registry</description></item>
        /// <item><description>Stores the handler locally for later invocation</description></item>
        /// </list>
        /// The observer must call <see cref="SubscribeAll"/> after registering handlers.
        /// </remarks>
        void Register<TContainer>(Action<TContainer> handler)
            where TContainer : class;
    }
}