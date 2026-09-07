using Sasha19.Enums;
using Sasha19.GameBootstrapper;
using UnityEngine;
using Zenject;

namespace Sasha19.Register
{
    public class DomainProjectInstaller : MonoInstaller<DomainProjectInstaller>
    {
        public override void InstallBindings()
        {
            CoreGameBotsrappInstaller.Install(Container);
            Container.Bind<IBootstrappStep>().WithId(SystemsCoreStepType.FoundationStep).To<SaveSystemTest>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameStepsRegister>().FromNew().AsSingle();
            Debug.Log("s");
        }
    }
}
