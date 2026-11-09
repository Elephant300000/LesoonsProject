using Lucky38.UnitReact.Core;
namespace Sasha19.Entity.MyPlayer
{
    public interface IPlayerModel
    {
        ReactiveProperty<PlayerData> reactivePlayerData { get; }
        void UpdatePlayerData(PlayerData newPlayerData);
    }
}