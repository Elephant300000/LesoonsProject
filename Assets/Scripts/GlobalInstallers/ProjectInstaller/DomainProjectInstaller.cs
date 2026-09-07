using UnityEngine;
using Zenject;

namespace Sasha19.Register
{
    public class DomainProjectInstaller : MonoInstaller<DomainProjectInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CoreGameBootstrapper>().FromNew().AsSingle().NonLazy();
            Container.Bind<IBootstrappStep>().WithId(SystemsCoreStepType.FoundationStep).To<SaveSystemTest>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameStepsRegister>().FromNew().AsSingle();
            Debug.Log("s");
        }
    }
}
