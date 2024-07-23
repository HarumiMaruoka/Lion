using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Minion.UI
{
    [RequireComponent(typeof(Button))]
    public class MinionWindowOpenButton : MonoBehaviour
    {
        [SerializeField] private MinionWindow _minionWindow;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _minionWindow.Open();
        }
    }
}