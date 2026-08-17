namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Represents an event container that can be reused through an object pool.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface marks an event type as compatible with pooling mechanisms
    /// such as <see cref="EventPool{T}"/> and pooled event subjects.
    /// </para>
    ///
    /// <para>
    /// Implementations are expected to restore their internal state to a clean,
    /// reusable default inside <see cref="Release"/>.
    /// </para>
    ///
    /// <para>
    /// <b>Lifecycle contract:</b>
    /// <list type="number">
    /// <item><description>The event instance is retrieved from a pool</description></item>
    /// <item><description>The instance is populated and dispatched</description></item>
    /// <item><description><see cref="Release"/> is called after dispatch</description></item>
    /// <item><description>The instance is returned to the pool</description></item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// <see cref="Release"/> must be:
    /// <list type="bullet">
    /// <item><description>Safe to call exactly once per lifecycle</description></item>
    /// <item><description>Free of allocations</description></item>
    /// <item><description>Independent of external state</description></item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// The pool itself does not enforce correctness — the caller is responsible
    /// for invoking <see cref="Release"/> at the appropriate time.
    /// </para>
    /// </remarks>
    public interface IPoolableEvent
    {
        void Release();
    }
    public interface IStandardEvent { }
}
