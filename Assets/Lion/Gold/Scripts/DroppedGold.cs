using System;
using UnityEngine;

namespace Lion.Gold
{
    public class DroppedGold : MonoBehaviour
    {
        // �擾���邱�Ƃœ���ł���S�[���h�̗ʁB
        [field: SerializeField] public int Amount { get; set; } = 1;

        public DroppedGoldPool Pool { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IGoldCollector>(out var collector))
            {
                GoldManager.Instance.Earn(Amount);
                collector.CollectGold(Amount);
                Pool.Deactivate(this);
            }
        }
    }
}