using Sasha19.DataInterfases;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStepsRegister : IGameStepsRegister
{
    private Dictionary<Type, List<IStepMarker>> _steps = new();

    bool IGameStepsRegister.TryGetStep<TStep>(out TStep step)
    {

       if(_steps.TryGetValue(typeof(TStep), out var steps) & steps.Count>0)
       {
           step = (TStep) steps[0];
           return true;
       }
       step = null;
       return false;
    }

    IEnumerable<TStep> IGameStepsRegister.GetSteps<TStep>()
    {
        if (_steps.TryGetValue(typeof(TStep), out var steps))
        {
            return steps.OfType<TStep>();
        }
        return Enumerable.Empty<TStep>();
    }

    void IGameStepsRegister.RegisterStep<TStep>(TStep step)
    {
        var steps = new List<IStepMarker>();
        if (step != null && !_steps.TryGetValue(typeof(TStep), out steps))
        {
            steps = new List<IStepMarker>();
            _steps[typeof(TStep)] = steps;
        }
        if (steps.Contains(step))
        {
            return;
        }
        steps.Add(step);
    }

    void IGameStepsRegister.UnRegisterStep<TStep>(TStep step)
    {
        var steps = new List<IStepMarker>();
        if (step == null || !_steps.TryGetValue(typeof(TStep), out steps))
        {
            Debug.LogWarning("List is empty");
            return;
        }
        if(!steps.Remove(step))
        {
            Debug.LogWarning("List is empty");
        }
        
    }
}



public interface IGameStepsRegister
{
    void RegisterStep<TStep>(TStep step) where TStep : class, IStepMarker;
    void UnRegisterStep<TStep>(TStep step) where TStep : class, IStepMarker;
    bool TryGetStep<TStep>(out TStep step) where TStep : class, IStepMarker;
    IEnumerable<TStep> GetSteps<TStep>() where TStep : class, IStepMarker;
}