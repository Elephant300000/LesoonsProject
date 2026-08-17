using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MonoSaveInstaller : MonoInstaller
{
    
              
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<FileSaveServise>().FromNew().AsSingle().WithArguments(Application.persistentDataPath);
        Container.BindInterfacesAndSelfTo<SaveDataOrcestrator>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<PerFirst>().FromComponentInHierarchy().AsCached().NonLazy();
        Container.BindInterfacesAndSelfTo<PerSecond>().FromComponentInHierarchy().AsCached().NonLazy();
        Container.BindInterfacesAndSelfTo<PerThird>().FromComponentInHierarchy().AsCached().NonLazy();
    }
}