using Lucky38.UnitReact.Core;
using Zenject;

public class HudInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ButtonTrigger>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<HudBur>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<HudBarObserver>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo <Character>().FromComponentInHierarchy().AsSingle();
 
    }
}
