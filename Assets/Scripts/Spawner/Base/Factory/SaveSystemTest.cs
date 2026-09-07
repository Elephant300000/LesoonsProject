using Cysharp.Threading.Tasks;
using Sasha19.GameBootstrapper;
using System.Threading;
using UnityEngine;

public class SaveSystemTest : IBootstrappStep
{
    public UniTask ExicuteAsync(CancellationToken token)
    {
        Debug.Log("Object saved and registered");
        return UniTask.CompletedTask;
    }
}