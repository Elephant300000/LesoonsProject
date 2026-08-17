using System;

namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Интерфейс только для чтения. Позволяет подписаться на изменение и получить значение, 
    /// но не дает изменять его извне.
    /// </summary>
    public interface IReadOnlyReactiveProperty<out T> : ISubscribable<T>, IDisposable
    {
        T Value { get; } 
    }
     
   
}