using Lucky38.UnitReact.Core;
using System;
using UnityEngine;
using Zenject;

namespace Lucky38.TestReact.EntityScene
{
    public class Entity : MonoBehaviour
    {
        private Transform _tr => transform; 
        public readonly int Id = Guid.NewGuid().GetHashCode(); 

        public EntityData Data { get; private set; }
         
        public SubjectEntity Subject { get; private set; }

        private ReactiveProperty<EntitySelectable> _selectedEntity;
        private RepositoryEntity _repository;
        private ReactiveProperty<float> _playerHp = new();
        public IReadOnlyReactiveProperty<float> PlayerHp=> _playerHp;

        [Inject]
        public void Construct(
            ReactiveProperty<EntitySelectable> selectedEntity,  
            RepositoryEntity repository,
            SubjectEntity subject)
        {
            _selectedEntity = selectedEntity;
            _repository = repository;
            Subject = subject;
        }

        private void Start()
        {
            Data = new EntityData(Id, 0.5f);
            _repository.Register(this);
        }

        public void SetData(EntityData newData)
        {
            Data = newData;
        }

        private void OnDestroy()
        {
            _repository.Unregister(this);
        }

        public void OnMouseDown()
        {
            _tr.localScale = Vector3.one * 1.5f;
            _selectedEntity.Value = new EntitySelectable(Id);
            _playerHp.Value += 1;
        }
        public void OnMouseExit()
        {
            _tr.localScale = new Vector3(1, 1, 1);
        }
        
    }
}