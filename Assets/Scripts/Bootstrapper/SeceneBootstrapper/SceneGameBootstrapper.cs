using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;
internal class SceneGameBootstrapper : CoreGameBootstrapperBase
{
    public SceneGameBootstrapper([Inject(Id = SystemsGameStepType.SceneLoadStep)] List<IBootstrappStep> SceneLoadStep,
        [Inject(Id = SystemsGameStepType.PostLoadStep)] List<IBootstrappStep> PostLoadStep) : base(SceneLoadStep, PostLoadStep)
    {
        
        Debug.Log("Scene");
    }
}
public enum SystemsGameStepType
{
    SceneLoadStep,
    InfrastructureServisesStep,
    ServiseStep,
    PostLoadStep,

} 