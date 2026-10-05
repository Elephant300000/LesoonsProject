using System;
using UnityEngine;

public struct PlayerData 
{
    public uint PlayerId;
    public uint IdConfige;
    public Vector2 PositionV2;
    public Vector3 PositionV3;
    public Quaternion Rotation;
    public float CurrentHp;
    public float MaxHp;

    public PlayerData(uint playerId, uint idConfige, Vector2 positionV2, Vector3 positionV3, Quaternion rotation, float currentHp, float maxHp)
    {
        PlayerId = playerId;
        IdConfige = idConfige;
        PositionV2 = positionV2;
        PositionV3 = positionV3;
        Rotation = rotation;
        CurrentHp = currentHp;
        MaxHp = maxHp;
    }
    public PlayerData With(uint? playerId = null, uint? idConfige = null, Vector2? positionV2 = null, Vector3? positionV3 = null, Quaternion? rotation = null, float? currentHp = null, float? maxHp = null)
    {
        return new(PlayerId,
            IdConfige,
            positionV2?? PositionV2,
            positionV3?? PositionV3,
            rotation?? Rotation,
            currentHp?? CurrentHp,
            maxHp?? MaxHp);
    }
}
