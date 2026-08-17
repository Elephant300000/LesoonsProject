using Cysharp.Threading.Tasks;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Defines an asynchronous event subject.
    /// </summary>
    /// <remarks>
    /// An async event subject dispatches events to observers using asynchronous
    /// handlers (<see cref="UniTask"/> based).
    ///
    /// The subject:
    /// - Manages observer subscriptions
    /// - Validates invocation signatures
    /// - Invokes async action and function handlers
    ///
    /// Signature registration must be performed by observers before invocation.
    /// </remarks>
    public interface IAsyncEventSubject: IEventSubjectCore, IActionSignatureRegistry
    {
        /// <summary>
        /// Invokes all asynchronous action handlers for the given event.
        /// </summary>
        UniTask InvokeActionAsync<TContainer>(TContainer container) where TContainer : class;
    }
}
