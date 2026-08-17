using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Lucky38.UnitReact.Core
{ 
    /// <summary>
    /// Defines the ability to handle asynchronous events.
    /// </summary>
    /// <remarks>
    /// This interface represents the execution phase of asynchronous
    /// event dispatch.
    ///
    /// Subjects use this interface to invoke async handlers
    /// and await their completion.
    ///
    /// Implementations must ensure that returned tasks represent
    /// the full lifetime of the handler execution.
    /// </remarks>
    public interface IAsyncObserverHandler : IObserver
    {
        /// <summary>
        /// Invokes an asynchronous action handler.
        /// </summary>
        /// <typeparam name="TContainer">The event type.</typeparam>
        /// <param name="ev">The event instance.</param>
        /// <returns>A failed <see cref="UniTask"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Always returned as a failed task in this base class.
        /// </exception>
        UniTask ReactAsync<TContainer>(TContainer evt, CancellationToken token) where TContainer : class;
    }
}