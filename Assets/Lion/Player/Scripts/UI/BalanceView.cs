using Lion.Gold;
using System;
using TMPro;
using UnityEngine;

namespace Lion.Player.UI
{
    public class BalanceView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _balanceText;

        private int Balance => GoldManager.Instance.Balance;

        private void Start()
        {
            UpdateBalance(Balance);
            GoldManager.Instance.OnBalanceChanged += UpdateBalance;
        }

        private void OnDestroy()
        {
            GoldManager.Instance.OnBalanceChanged -= UpdateBalance;
        }

        private void UpdateBalance(int value)
        {
            _balanceText.text = value.ToString();
        }
    }
}