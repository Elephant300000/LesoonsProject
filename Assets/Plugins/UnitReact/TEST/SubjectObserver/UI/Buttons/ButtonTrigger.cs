using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lucky38.TestReact.EntityScene
{
    public class ButtonTrigger : MonoBehaviour
    {
        public Button btIncrement;
        public Button btDecrement;

        public event Action OnIncrementClicked;
        public event Action OnDecrementClicked;

        private void Awake()
        {
            btIncrement.onClick.AddListener(() => OnIncrementClicked?.Invoke());
            btDecrement.onClick.AddListener(() => OnDecrementClicked?.Invoke());
        }

        private void OnDestroy()
        {
            btIncrement.onClick.RemoveAllListeners();
            btDecrement.onClick.RemoveAllListeners();
        }
    }
}