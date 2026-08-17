using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Lucky38.UnitReact.Core
{ 
    /// <summary>
    /// Defines the ability to register asynchronous event handlers.
    /// </summary>
    /// <remarks>
    /// This interface is implemented by observers that support
    /// asynchronous event handling using <see cref="UniTask"/>.
    ///
    /// Not all observers are required to support async handlers.
    /// Base observer implementations may explicitly reject async registration.
    /// </remarks>
    public interface IAsyncObserverRegistrar : IObserver
    {
        /// <summary>
        /// Registers an asynchronous action handler.
        /// </summary>
        /// <typeparam name="TContainer">The event type.</typeparam>
        /// <param name="handler">The async handler.</param>
        /// <exception cref="InvalidOperationException">
        /// Always thrown in this base class.
        /// </exception>
        /// <remarks>
        /// Asynchronous handlers are not supported in <see cref="ObserverContextBase"/>.
        /// Derive from an async-capable observer to enable this functionality.
        /// </remarks>
        void RegisterAsync<TContainer>(Func<TContainer, CancellationToken, UniTask> handler)
            where TContainer : class;
    }
}