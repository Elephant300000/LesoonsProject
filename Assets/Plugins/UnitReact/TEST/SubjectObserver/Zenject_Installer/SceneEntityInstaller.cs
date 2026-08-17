using Lucky38.UnitReact.Core;
using UnityEngine;
using Zenject;

namespace Lucky38.TestReact.EntityScene
{
    public class SceneEntityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Находим и биндим UI элементы со сцены
            Container.Bind<HudEntity>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ButtonTrigger>().FromComponentInHierarchy().AsSingle();
            
            // Глобальный репозиторий сущностей
            Container.Bind<RepositoryEntity>().AsSingle();
            Container.Bind<Entity>().FromComponentInHierarchy().AsSingle();
            
            // Глобальное состояние (выбранный ID)
            var initialSelection = new ReactiveProperty<EntitySelectable>(new EntitySelectable(-1));
            Container.BindInterfacesAndSelfTo<ReactiveProperty<EntitySelectable>>().FromInstance(initialSelection).AsSingle();

            // Глобальный обработчик инпута и UI-кнопок
            Container.BindInterfacesAndSelfTo<EntityHandler>().AsSingle().NonLazy();
        }
    }
}