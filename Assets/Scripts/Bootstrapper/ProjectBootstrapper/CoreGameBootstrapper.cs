using System.Collections.Generic;
using UnityEngine;
using Zenject;

internal class CoreGameBootstrapper : CoreGameBootstrapperBase
{
    public CoreGameBootstrapper([Inject(Id = SystemsCoreStepType.FoundationStep)] List<IBootstrappStep> FoundationStep,
    [Inject(Id = SystemsCoreStepType.GlobalInitStep)] List<IBootstrappStep> GlobalInitStep) : base(FoundationStep, GlobalInitStep)
    {
        Debug.Log("Game");
    }
}
public enum SystemsCoreStepType
{
    GlobalInitStep,
    FoundationStep,
    ServiseStep,
    LoadStep
}