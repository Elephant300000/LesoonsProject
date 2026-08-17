namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Optional contract for non-pooled events that need a durable history copy.
    /// Used by <see cref="StandardReplayBuffer{TContainer}"/> when replay is enabled.
    /// </summary>
    public interface ICloneableEvent<T> where T : class
    {
        T Clone();
    }
}
