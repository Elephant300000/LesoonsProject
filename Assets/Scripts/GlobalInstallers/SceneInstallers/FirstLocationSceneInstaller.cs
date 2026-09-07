using Sasha19.GameBootstrapper;
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
        SceneBotsrappInstaller.Install(Container);
        Container.Bind<PlayerInput>().FromInstance(inputActions).AsSingle();
        Container.Bind<IBootstrappStep>().WithId(SystemsGameStepType.PostLoadStep).To<SpawnerTestStep>().FromComponentInHierarchy().AsSingle();
       
    }
}