using Sasha19.GameBootstrapper;
using UnityEngine;
using Zenject;

public class CoreGameBotsrappInstaller : Installer<CoreGameBotsrappInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CoreGameBootstrapper>().FromNew().AsSingle().NonLazy();

    }
}