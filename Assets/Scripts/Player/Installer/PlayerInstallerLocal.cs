using Sasha19.Entity.MyPlayer;
using Sasha19.Enums;
using Sasha19.MainShared.Configs;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInstallerLocal : MonoInstaller
{

    [SerializeField]
    private Rigidbody _playerRb;
    [SerializeField]
    private PlayerMoveProperty _moveProperty;
    public override void InstallBindings()
    {
        Container.Bind<Rigidbody>().WithId(ZenjectsId.PlayeRb).FromInstance(_playerRb).AsCached();
        Container.BindInterfacesAndSelfTo<PlayerMoveProperty>().FromInstance(_moveProperty).AsSingle();
        Container.BindInterfacesAndSelfTo<MovementPlayerInput>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerFaced>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerUIObserver>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerMovement>().FromNew().AsSingle().NonLazy();
    }
}