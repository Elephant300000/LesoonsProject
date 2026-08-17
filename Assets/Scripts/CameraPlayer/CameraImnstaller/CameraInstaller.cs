using Lucky38.MyCamera;
using UnityEngine;
using Zenject;

public class CameraInstaller : Installer<CameraInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CinemaCameraDriver>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<CinemaCameraCollision>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<CinemaCameraControlService>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<MainCameraTransformPose>().FromNew().AsSingle();
    }
}