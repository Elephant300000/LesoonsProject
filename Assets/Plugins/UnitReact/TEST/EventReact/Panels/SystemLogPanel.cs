using System;
using System.Text;
using Lucky38.UnitReact.Core;
using TMPro;
using UnityEngine;

namespace Lucky38.TestReact.EventBusDemo
{
    public enum WorldSystemKind
    {
        Combat = 0,
        Vfx = 1,
        Audio = 2,
        Quest = 3
    }

    /// <summary>
    /// Независимая система-панель (Combat / VFX / Audio / Quest).
    /// Слушает FastEventHub (gameplay) и MessageBroker (UI-логи других систем).
    /// Нет ссылок на кнопку и на другие панели.
    /// </summary>
    public sealed class SystemLogPanel : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField] private WorldSystemKind systemKind = WorldSystemKind.Combat;
        [SerializeField] private string channelName = "Combat";

        [Header("View")]
        [SerializeField] private TextMeshProUGUI logText; 

        private CompositeDisposable _bag = new();

        private void OnEnable()
        { 

            FastEventHubRx.Receive<WorldPulseEventData>()
                    .Gate(1)
                    .Where(d => d.Tick > 0)
                    .Take(8)
                    .Skip(1) 
                    .Subscribe(OnWorldPulse)
                    .AddTo(_bag);

            MessageBroker.Receive<UiLogMessage>()
                      .Gate(0.5f)
                      .Where(ValidationMassage)
                      .Take(11)
                      .Skip(1)
                      .Subscribe(OnForeignUiLog)
                      .AddTo(_bag); 
        }

        private void OnDisable()
        {
            _bag?.Dispose();
            _bag = null;
        }
        public bool ValidationMassage(UiLogMessage message)
        {
            print(message.AmountMassage);
            return message.AmountMassage <= 10;
        }
        private void OnWorldPulse(WorldPulseEventData pulse)
        {
            var sourceLog = $"WorldPulse - {pulse.Source}," +
                $" Intensity -  {pulse.Intensity}," +
                $"Tick - {pulse.Tick}";
             
            AppendLocal(sourceLog);
            print("Global eventHub");
        }

        private void OnForeignUiLog(UiLogMessage msg)
        {
            AppendLocal($"← {msg.Channel}: {msg.Text}");
            print("Global eventMessage");
        }

        private void AppendLocal(string line)
        {
            if (logText == null)
                return;
            logText.text = line;
        }

    
    }
}
