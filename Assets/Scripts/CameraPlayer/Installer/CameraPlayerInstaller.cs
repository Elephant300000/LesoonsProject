using Lucky38.MyCamera;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CameraPlayerInstaller : Installer<CameraPlayerInstaller>
{

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CinemaCameraControlService>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<CameraInput>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<CinemaCameraDriver>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<CinemaCameraCollision>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<MainCameraTransformPose>().FromNew().AsSingle();
        

    }
}