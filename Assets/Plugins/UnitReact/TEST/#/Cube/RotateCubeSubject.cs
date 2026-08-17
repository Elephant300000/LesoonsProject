using Cysharp.Threading.Tasks;
using Lucky38.UnitReact.Core;
using UnityEngine;
using Zenject;

public class RotateCubeSubject : PooledAsyncEventSubject, ITickable
{
    private readonly float rotationSpeed = 45f;
    private readonly float rotationDuration = 5f;

    public RotateCubeSubject()
    {
        ConfigureAsyncGate<CubeEventData>(cooldownSeconds: 0f, skipIfRunning: true);
        EnablePooledReplay<CubeEventData>(capacity: 1);
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.R))
            OnStartRotate(rotationDuration, rotationSpeed).Forget();
    }

    public UniTask OnStartRotate(float duration, float speed)
    {
        return InvokePooledActionAsync<CubeEventData>(e =>
        {
            e.RotationSpeed = speed;
            e.RotationDuration = duration;
        });
    }
}
