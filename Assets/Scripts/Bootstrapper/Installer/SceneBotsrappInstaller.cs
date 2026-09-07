using UnityEngine;
using Zenject;

public class SceneBotsrappInstaller : Installer<SceneBotsrappInstaller>
{
    public override void InstallBindings()
    {

        Container.BindInterfacesAndSelfTo<SceneGameBootstrapper>().FromNew().AsSingle().NonLazy();
    }
}