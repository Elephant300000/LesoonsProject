using Cysharp.Threading.Tasks;
using Lucky38.UnitReact.Core;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// Hang on each cube. All instances receive the same rotate event.
/// </summary>
public class CubeRotateAsync : MonoBehaviour
{
    [SerializeField] private Transform cube;

    private void Awake()
    {
        if (cube == null)
            cube = transform;
    }

    public async UniTask RotateAsync(CubeEventData eventData, CancellationToken token)
    { 
        float elapsed = 0f; 
        try
        {
            while (elapsed < eventData.RotationDuration)
            {
                token.ThrowIfCancellationRequested();

                float delta = Time.deltaTime;
                elapsed += delta;

                if (cube != null)
                    cube.Rotate(Vector3.up * eventData.RotationSpeed * delta);

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
        catch (OperationCanceledException)
        {
        }
    } 
    
}

public sealed class CubeEventData : IPoolableEvent
{
    public float RotationDuration;
    public float RotationSpeed;

    public void Release()
    {
        RotationDuration = default;
        RotationSpeed = default;
    }
}
