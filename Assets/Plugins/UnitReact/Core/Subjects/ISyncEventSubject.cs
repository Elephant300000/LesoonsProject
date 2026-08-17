namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Defines a synchronous event subject.
    /// </summary>
    /// <remarks>
    /// A synchronous event subject dispatches events to observers immediately
    /// using synchronous handlers.
    ///
    /// This interface represents the simplest execution model and is suitable
    /// when asynchronous execution is not required.
    /// </remarks>
    public interface ISyncEventSubject : IEventSubjectCore, IActionSignatureRegistry
    {
        /// <summary>
        /// Invokes all synchronous action handlers for the given event.
        /// </summary>
        void InvokeAction<TContainer>(TContainer container) where TContainer : class;
    }
}
