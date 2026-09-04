using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

internal abstract class CoreGameBootstrapperBase : IInitializable, IDisposable
{
    private readonly CancellationTokenSource _tokenSourse;
    private readonly IReadOnlyList<IReadOnlyList<IBootstrappStep>> _phases;

    public CoreGameBootstrapperBase(params IReadOnlyList<IBootstrappStep>[] phases)
    {
        _tokenSourse = new();
        _phases = phases ?? Array.Empty<IReadOnlyList<IBootstrappStep>>();
    }

    public void Dispose()
    {
        _tokenSourse?.Cancel();
        _tokenSourse?.Dispose();
    }

    public void Initialize()
    {
        RunBootstrappAsync(_tokenSourse.Token).Forget();
        Debug.Log("Game started");
    }

    private async  UniTask RunBootstrappAsync(CancellationToken token)
    {
        try
        {
            foreach (var phase in _phases)
            {
                await PhaseAsyncExicute(phase, token);
            }
        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }

    }
    private async UniTask PhaseAsyncExicute( IReadOnlyList<IBootstrappStep> bootstrappSteps, CancellationToken token)
    {       
        if (bootstrappSteps == null || bootstrappSteps.Count == 0)
        {
            return;
        }
        foreach (var step in bootstrappSteps)
        {
            if (step == null)
            {
                continue;
            }
            token.ThrowIfCancellationRequested();
            await step.ExicuteAsync(token);
        }
    }
}
public interface IBootstrappStep
{
    string StepName => GetType().Name;
    UniTask ExicuteAsync(CancellationToken token);    
}