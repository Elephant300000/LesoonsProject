using Lucky38.UnitReact.Core;
using UnityEngine;

public class PlayerFacedProvaider : MonoBehaviour, IPlayerFacedProvaiderSet, IPlayerFacedProvaiderGet
{
    public IPlayerFaced PlayerFaced;

    public void PlayerFacedSet(IPlayerFaced playerFaced)
    {
        PlayerFaced = playerFaced;
    }

    public IPlayerFaced PlayerFasedGet()
    {
        return PlayerFaced;
    }
}
public interface IPlayerFacedProvaiderGet
{
    IPlayerFaced PlayerFasedGet();
}
public interface IPlayerFacedProvaiderSet
{
    void PlayerFacedSet(IPlayerFaced playerFaced);
}
public interface IPlayerFaced
{
    ReactiveProperty<PlayerData> reactivePlayerData { get; }
    void UpdatePlayerData(PlayerData newPlayerData);
}