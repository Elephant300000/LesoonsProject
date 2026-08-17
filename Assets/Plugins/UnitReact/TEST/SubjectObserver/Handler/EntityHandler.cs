using System;
using UnityEngine;
using Zenject;
using Lucky38.UnitReact.Core;

namespace Lucky38.TestReact.EntityScene
{ 
    public class EntityHandler : IInitializable, IDisposable
    {
        private readonly ButtonTrigger _buttons;
        private readonly RepositoryEntity _repository;

        private readonly IReadOnlyReactiveProperty<EntitySelectable> _selectedEntity;
        private readonly CompositeDisposable _disposables = new();
        private readonly Entity _entity;

        public EntityHandler(
            IReadOnlyReactiveProperty<EntitySelectable> selectedEntity,
            ButtonTrigger buttons,
            RepositoryEntity repository,
            Entity entity)
        {
            _selectedEntity = selectedEntity;
            _buttons = buttons;
            _repository = repository;
            _entity = entity;
        }

        public void Initialize()
        {
            _entity.PlayerHp
                .Subscribe(Pr)
                .AddTo(_disposables); 
            _selectedEntity.Skip(1)
                .Take(2)
                .Subscribe(OnSelectionChanged).AddTo(_disposables);
    
            EventBinding.FromEvent<Unit>(
                h => _buttons.OnIncrementClicked += () => h(Unit.Default),
                h => _buttons.OnIncrementClicked -= () => h(Unit.Default))
                .Gate(cooldownSeconds: 0.2f)
                .Subscribe(OnIncrement)
                .AddTo(_disposables);

            EventBinding.FromEvent<Unit>(
                h => _buttons.OnDecrementClicked += () => h(Unit.Default),
                h => _buttons.OnDecrementClicked -= () => h(Unit.Default))
                .Gate(cooldownSeconds: 0.2f)
                .Subscribe(OnDecrement)
                .AddTo(_disposables);
        }

        private void OnSelectionChanged(EntitySelectable selection)
        {
            var entity = _repository.Get(selection.Id);
            if (entity != null)
            {  
                entity.Subject.NotifyState();
            }
        }

        private void OnIncrement()
        {
            var entity = _repository.Get(_selectedEntity.Value.Id);
            if (entity != null)
            { 
                entity.Subject.ChangeAmount(0.1f);
            }
        }

        private void OnDecrement()
        {
            var entity = _repository.Get(_selectedEntity.Value.Id);
            if (entity != null)
            { 
                entity.Subject.ChangeAmount(-0.1f);
            }
        }

        public void Dispose()
        {
            _disposables.Dispose(); 
        }
        private void Pr(float f)
        {
            Debug.Log("Hp changed");
        }
    }
}