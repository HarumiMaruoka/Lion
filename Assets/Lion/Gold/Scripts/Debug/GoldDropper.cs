using Lion.Gold;
using System;
using UnityEngine;

namespace Lion.LionDebugger
{
    public class GoldDropper : MonoBehaviour
    {
        [SerializeField]
        private DroppedGoldPool _droppedGoldPool;
        [SerializeField]
        private int _count = 5;
        [SerializeField]
        private float _range = 5f;
        [SerializeField]
        private int _minGoldValue = 1;
        [SerializeField]
        private int _maxGoldValue = 100;

        private void Start()
        {
            for (int i = 0; i < _count; i++)
            {
                Vector3 position = transform.position + new Vector3(UnityEngine.Random.Range(-_range, _range), UnityEngine.Random.Range(-_range, _range), 0);
                int goldValue = UnityEngine.Random.Range(_minGoldValue, _maxGoldValue);
                _droppedGoldPool.CreateDroppedGold(null, position, goldValue);
            }
        }
    }
}