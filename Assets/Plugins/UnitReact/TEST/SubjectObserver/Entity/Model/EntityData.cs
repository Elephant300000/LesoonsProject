using Lucky38.UnitReact.Core;

namespace Lucky38.TestReact.EntityScene
{
    public readonly struct EntityData
    {
        public EntityData(int id, float fildAmount)
        {
            Id = id;
            FildAmount = fildAmount;
        }

        public int Id { get; }  
        public float FildAmount { get; }
    }

    public class EntityUpdatedEvent : IPoolableEvent
    {
        public EntityData Data;
        public void Release() => Data = default;
    }
}