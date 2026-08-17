using Lucky38.UnitReact.Core;
using Zenject;

namespace Lucky38.TestReact.EntityScene
{
    public class HudEntityObserver : SyncObserverContext, IInitializable
    { 
        public HudEntityObserver(SubjectEntity subject, HudEntity hud) : base(subject, subject)
        { 
            _hud = hud;
        }
        private readonly HudEntity _hud;
        public void Initialize()
        { 
            Register<EntityUpdatedEvent>(OnEntityUpdated);
            SubscribeAll();
        }

        private void OnEntityUpdated(EntityUpdatedEvent evt)
        { 
            if (_hud.scrollbar != null)
                _hud.scrollbar.size = evt.Data.FildAmount;
        }
    }
}