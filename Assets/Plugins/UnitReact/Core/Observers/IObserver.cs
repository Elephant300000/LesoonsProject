namespace Lucky38.UnitReact.Core { 
    /// <summary>
    /// Represents an observer participating in the event system.
    /// </summary>
    /// <remarks>
    /// An observer is an object that:
    /// - Registers handlers for one or more event types
    /// - Reacts to events dispatched by a subject
    ///
    /// This interface is a compositional root that groups together
    /// synchronous and asynchronous observer capabilities.
    ///
    /// Concrete observer implementations typically inherit from
    /// a shared base class rather than implementing this interface directly.
    /// </remarks>
    public interface IObserver
    {

    }
}