using Lucky38.UnitReact.Core;
using Sasha19.Entity.MyPlayer;
using UnityEngine;

public class PlayerFacedProvaider : IPlayerFacedProvaiderSet, IPlayerFacedProvaiderGet
{
    public IPlayerModel PlayerFaced;

    public void PlayerFacedSet(IPlayerModel playerFaced)
    {
        PlayerFaced = playerFaced;
    }

    public IPlayerModel PlayerFasedGet()
    {
        return PlayerFaced;
    }
}
public interface IPlayerFacedProvaiderGet
{
    IPlayerModel PlayerFasedGet();
}
public interface IPlayerFacedProvaiderSet
{
    void PlayerFacedSet(IPlayerModel playerFaced);
}
