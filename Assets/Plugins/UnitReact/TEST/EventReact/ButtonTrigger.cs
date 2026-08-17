
    using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lucky38.TestReact.EventBusDemo
{
    public class ButtonTrigger : MonoBehaviour
    {
        public Button bt;
        private void Awake()
        {
            bt = GetComponent<Button>();
        }
        public void OnSetap(Action OnFireEvent)
        {
            if(bt != null)
            {
                bt.onClick.RemoveAllListeners();
                bt.onClick.AddListener(OnFireEvent.Invoke);
            }
        }
        private void OnDisable()
        {
            if (bt != null)
            {
                bt.onClick.RemoveAllListeners();
             }
        }
    }
}
