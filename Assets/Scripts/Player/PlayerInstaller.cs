using Sasha19.Entity.MyPlayer;
using Zenject;
public class PlayerInstaller : Installer<PlayerInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PlayerFacedProvaider>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<StatDisplayWidget>().FromComponentInHierarchy().AsSingle();
   
    }
}