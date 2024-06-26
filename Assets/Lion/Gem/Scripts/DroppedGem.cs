using System;
using UnityEngine;

namespace Lion.Gem
{
    public class DroppedGem : MonoBehaviour
    {
        // 取得することで入手できるExpの量。
        [field: SerializeField] 
        public int Amount { get; set; } = 1;

        public DroppedGemPool Pool { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IGemCollector>(out var collector))
            {
                collector.CollectGem(Amount);
                Pool.Deactivate(this);
            }
        }
    }
}