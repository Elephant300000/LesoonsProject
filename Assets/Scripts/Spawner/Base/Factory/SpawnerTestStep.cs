using Cysharp.Threading.Tasks;
using Sasha19.GameBootstrapper;
using System.Threading;
using UnityEngine;

public class SpawnerTestStep : MonoBehaviour, IBootstrappStep
{
    [SerializeField]
    private GameObject block;
    public UniTask ExicuteAsync(CancellationToken token)
    {
        Instantiate(block);
        return UniTask.CompletedTask;
    }
}
