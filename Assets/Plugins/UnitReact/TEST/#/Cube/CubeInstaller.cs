using Lucky38.UnitReact.Core.Tests;
using Zenject;

public class CubeInstaller : MonoInstaller
{
    public override void InstallBindings()
    { 
        Container.Bind<CubeRotateAsync>()
            .FromComponentsInHierarchy()
            .AsCached();

        Container.BindInterfacesAndSelfTo<RotateCubeObserver>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<RotateCubeSubject>().AsSingle().NonLazy();

        var battle = FindObjectOfType<UnityReactBattleTest>();
        if (battle != null)
        {
            Container.BindInterfacesAndSelfTo<UnityReactBattleTest>()
                .FromInstance(battle)
                .AsSingle()
                .NonLazy();
        }
    }
}
