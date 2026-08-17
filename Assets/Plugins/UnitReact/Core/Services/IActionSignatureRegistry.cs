using System;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Defines a registry for event invocation signatures.
    /// </summary>
    /// <remarks>
    /// The signature registry ensures that:
    /// - Each event type is associated with exactly one delegate signature
    /// - Subjects and observers agree on how an event must be invoked
    ///
    /// Observers register expected signatures during handler registration.
    /// Subjects validate invocation attempts against the registered signatures.
    ///
    /// This mechanism prevents accidental mismatches such as:
    /// - Invoking an event as async when it was registered as sync
    /// - Mixing Action and Func semantics for the same event type
    /// </remarks>
    public interface IActionSignatureRegistry 
    {

        /// <summary>
        /// Registers the expected delegate signature for an event type.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <param name="delegateType">The delegate signature.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a different signature is already registered.
        /// </exception>
        void RegisterSignature(Type eventType, Type delegateType);
    }
}