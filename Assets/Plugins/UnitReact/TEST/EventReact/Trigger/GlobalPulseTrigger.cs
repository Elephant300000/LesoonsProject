using Lucky38.UnitReact.Core;
using UnityEngine;

namespace Lucky38.TestReact.EventBusDemo
{
    public sealed class GlobalPulseTrigger : MonoBehaviour
    {
        [SerializeField] private ButtonTrigger buttonTrigger;
        [SerializeField] private float gateCooldownSeconds = 0.35f;
        [SerializeField] private string pulseSource = "PlayerSkill";

        private int _tick; 

        private void Awake()
        {
            FastEventHubSync.ConfigureGate<WorldPulseEventData>(gateCooldownSeconds, skipIfRunning: true);
        }

        private void OnEnable()
        {
            if (buttonTrigger == null) return; 
            buttonTrigger.OnSetap(FirePulse);
        }

        private void FirePulse()
        { 
            _tick++;
            float intensity = 0.6f + (_tick % 5) * 0.1f;
             
            FastEventHubSync.Publish<WorldPulseEventData>(e =>
            {
                e.Tick = _tick;
                e.Intensity = intensity;
                e.Source = pulseSource;
            });

            MessageBroker.Publish(new UiLogMessage(_tick, pulseSource, "Hello World Message"));
        }
    }
}
