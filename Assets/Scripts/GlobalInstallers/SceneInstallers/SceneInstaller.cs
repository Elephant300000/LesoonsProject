using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class SceneInstaller : MonoInstaller
{

    [SerializeField]
    private PlayerInput inputActions;
    public override void InstallBindings()
    {
        CameraInstaller.Install(Container);
        PlayerInstaller.Install(Container);
        Container.Bind<PlayerInput>().FromInstance(inputActions).AsSingle();
    }
}