using Lucky38.UnitReact.Core;

namespace Lucky38.TestReact.EventBusDemo
{
    /// <summary>
    /// Gameplay-пульс: частый, pooled, gated.
    /// Не UI-текст — данные мира (урон / интенсивность / тик).
    /// </summary>
    public sealed class WorldPulseEventData : IPoolableEvent
    {
        public int Tick;
        public float Intensity;
        public string Source;

        public void Release()
        {
            Tick = 0;
            Intensity = 0f;
            Source = null;
        }
    }

    /// <summary>
    /// Лёгкое UI/лог-сообщение без пула.
    /// Системы шлют его после реакции на gameplay, не зная друг о друге.
    /// </summary>
    public readonly struct UiLogMessage
    {
        public UiLogMessage(int amountMassage, string channel, string text)
        {
            AmountMassage = amountMassage;
            Channel = channel;
            Text = text;
        }
        public int AmountMassage { get; }
        public string Channel { get; }
        public string Text { get; }
    }
}
