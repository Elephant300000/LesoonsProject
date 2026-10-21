using System;
using UnityEngine;

public struct PlayerData 
{
    public uint PlayerId;
    public uint IdConfige;
    public Vector2 PositionV2;
    public Vector3 PositionV3;
    public Quaternion Rotation;
    public StatsState StatsState;

    public PlayerData(uint playerId, uint idConfige, Vector2 positionV2, Vector3 positionV3, Quaternion rotation, StatsState statsState)
    {
        PlayerId = playerId;
        IdConfige = idConfige;
        PositionV2 = positionV2;
        PositionV3 = positionV3;
        Rotation = rotation;
        StatsState = statsState;
    }
    public PlayerData With(uint? playerId = null,
        uint? idConfige = null,
        Vector2? positionV2 = null,
        Vector3? positionV3 = null,
        Quaternion? rotation = null,
        StatsState? statsState = null)
    {
        return new(
            PlayerId,
            IdConfige,
            positionV2?? PositionV2,
            positionV3?? PositionV3,
            rotation?? Rotation,
            statsState?? StatsState
        );
    }

}
public readonly struct StatsState
{
    public readonly float CurrentHp;
    public readonly float MaxHp;

    public StatsState(float currentHp, float maxHp)
    {
        CurrentHp = currentHp;
        MaxHp = maxHp;
    }
}