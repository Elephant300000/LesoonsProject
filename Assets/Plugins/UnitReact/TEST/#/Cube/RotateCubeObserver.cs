using Cysharp.Threading.Tasks;
using Lucky38.UnitReact.Core;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class RotateCubeObserver : AsyncObserverContext, IInitializable
{
    private readonly List<CubeRotateAsync> _cubes;

    public RotateCubeObserver(RotateCubeSubject subject, List<CubeRotateAsync> cubes)
        : base(subject, subject)
    {
        _cubes = cubes;
    }

    public void Initialize()
    {
        RegisterAsync<CubeEventData>(RotateCubeAsync);
        SubscribeAll();
        
        Debug.Log($"[RotateCube] Subscribed {_cubes.Count} cube(s)");
    }
    private async UniTask RotateCubeAsync(CubeEventData cubeEventData, CancellationToken token)
    {
        if (_cubes == null || _cubes.Count == 0)
            return;

        var tasks = new UniTask[_cubes.Count];
        for (int i = 0; i < _cubes.Count; i++)
            tasks[i] = _cubes[i].RotateAsync(cubeEventData, token);

        await UniTask.WhenAll(tasks);
    }
}
