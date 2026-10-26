using System;
using Zenject;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInstaller : Installer<PlayerInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MovementPlayerInput>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerMovementComponent>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerFacedProvaider>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<StatDisplayWidget>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerUIObserver>().FromNew().AsSingle().NonLazy();
    }
}