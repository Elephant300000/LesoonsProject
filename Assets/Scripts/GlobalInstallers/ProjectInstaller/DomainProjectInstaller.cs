using UnityEngine;
using Zenject;

public class DomainProjectInstaller : MonoInstaller<DomainProjectInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CoreGameBootstrapper>().FromNew().AsSingle().NonLazy();
        Container.Bind<IBootstrappStep>().WithId(SystemsCoreStepType.FoundationStep).To<SaveSystemTest>().FromNew().AsSingle().NonLazy();        
        Debug.Log("s");
    }
}
