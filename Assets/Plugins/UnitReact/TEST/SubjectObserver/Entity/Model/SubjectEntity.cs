using Lucky38.UnitReact.Core;
using UnityEngine;
using Zenject;

namespace Lucky38.TestReact.EntityScene
{
    public class SubjectEntity : PooledSyncEventSubject 
    { 
        private readonly Entity _entityFacade; 

        public SubjectEntity(Entity entityFacade)
        {
            _entityFacade = entityFacade;
        }

        public int GetEntityId() => _entityFacade.Id;

        public void ChangeAmount(float delta)
        {  
            float newAmount = Mathf.Clamp01(_entityFacade.Data.FildAmount + delta); 
            var newData = new EntityData(_entityFacade.Id, newAmount);
             
            _entityFacade.SetData(newData);
             
            NotifyState(newData);
        }

        public void NotifyState()
        {
            NotifyState(_entityFacade.Data);
        }

        public void NotifyState(EntityData data)
        { 
            InvokePooledAction<EntityUpdatedEvent>(e => e.Data = data);
        }
    }
}
