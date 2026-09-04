using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class FirstLocationSceneInstaller : MonoInstaller
{

    [SerializeField]
  
    private PlayerInput inputActions;
    public override void InstallBindings()
    {
        CameraInstaller.Install(Container);
        PlayerInstaller.Install(Container);
        Container.Bind<PlayerInput>().FromInstance(inputActions).AsSingle();
        Container.BindInterfacesAndSelfTo<SceneGameBootstrapper>().FromNew().AsSingle().NonLazy();
        Container.Bind<IBootstrappStep>().WithId(SystemsGameStepType.PostLoadStep).To<SpawnerTestStep>().FromComponentInHierarchy().AsSingle();
    }
}