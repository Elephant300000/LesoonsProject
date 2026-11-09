using Sasha19.MainShared.Configs;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;
namespace Sasha19.Entity.MyPlayer
{
    [RequireComponent(typeof (Rigidbody))]
    internal class PlayerFaced : MonoBehaviour, IInitializable
    {

        private IPlayerModel _playerModel;
        private IPlayerFacedProvaiderSet _facedProvaiderSet;

        [Inject]
        public void Construct(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        public IPlayerMovement PlayerMovement { get; private set; }
        public void Initialize()
        {
            _facedProvaiderSet.PlayerFacedSet(_playerModel);
        }

        [Inject]
        public void Construct(IPlayerFacedProvaiderSet facedProvaider)
        {
            _facedProvaiderSet = facedProvaider;
        }

    }

    internal class PlayerHp : IInitializable, IPlayerStatProvaider, IDamageRecipient
    {
        private readonly IPlayerModel _playerModel;
        private readonly IPlayerStatsConfig _playerStatsConfig;
        public float _currentHp { get; private set; }

        public float _maxHp { get; private set; }
       
        public PlayerHp(IPlayerModel playerModel, IPlayerStatsConfig playerStatsConfig)
        {
            _playerModel = playerModel;
            _playerStatsConfig = playerStatsConfig;
        }

        public void Initialize()
        {
            _currentHp = _playerModel.reactivePlayerData.Value.StatsState.CurrentHp;
            _maxHp = _playerStatsConfig.playerStats.MaxHp;
        }

        void IDamageRecipient.TakeDamage(float changetHp)
        {

            var newPlData = _playerModel.reactivePlayerData.Value.With(new StatsState(_currentHp - changetHp, _maxHp));
            _playerModel.UpdatePlayerData(newPlData);
        }

     
    }
    public interface IPlayerStatProvaider
    {
        float _currentHp { get; }

        float _maxHp { get; }
    }
    public interface IDamageRecipient
    {
        void TakeDamage(float changetHp);

    }
}