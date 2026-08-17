namespace Lucky38.UnitReact.Core
{
    public interface ISyncEventGate  
    {
        bool TryEnter();

        void Exit();
    }
}