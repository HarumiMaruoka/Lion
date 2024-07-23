using Lion.Player;
using System;
using UnityEngine;

namespace Lion.UI
{
    public class BattlePowerView : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _text;

        private void Update()
        {
            _text.text = PlayerManager.Instance.BattlePower.ToString("0.0");
        }
    }
}