using Zenject;

namespace Lucky38.TestReact.EntityScene
{
    public class LocalEntityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Биндим компоненты с этого же GameObject (кубика)
            Container.Bind<Entity>().FromComponentOnRoot().AsSingle();
            
            // УНИКАЛЬНЫЙ локальный диспетчер событий для ЭТОГО кубика
            Container.Bind<SubjectEntity>().AsSingle();
            
            // Observer кубика для связи его локального Subject с глобальным HUD
            Container.BindInterfacesAndSelfTo<HudEntityObserver>().AsSingle().NonLazy();
        }
    }
}