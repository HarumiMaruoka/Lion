using System;
using UnityEngine;

namespace Lion.Gold
{
    public class GoldManager
    {
        public static GoldManager Instance { get; private set; } = new GoldManager();

        public int Balance { get; private set; } = 0;

        public event Action<int> OnBalanceChanged;

        public void Earn(int amount)
        {
            Balance += amount;
            OnBalanceChanged?.Invoke(Balance);
        }

        public void Pay(int amount)
        {
            Balance -= amount;
            OnBalanceChanged?.Invoke(Balance);
        }

        public bool CanPay(int amount)
        {
            return Balance >= amount;
        }
    }
}