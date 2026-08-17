namespace Lucky38.UnitReact.Core
{
    /// <summary>
    /// Структура-пустышка, представляющая отсутствие данных.
    /// Используется для сигналов (Trigger), чтобы они могли реализовывать ISubscribable&lt;T&gt;.
    /// </summary>
    public struct Unit 
    { 
        public static readonly Unit Default = new Unit();
    }
}