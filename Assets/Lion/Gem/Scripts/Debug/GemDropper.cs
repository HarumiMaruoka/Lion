using Lion.Gem;
using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Lin
{
    public class GemDropper : MonoBehaviour
    {
        [SerializeField]
        private DroppedGemPool _droppedGemPool;
        [SerializeField]
        private int _count = 5;
        [SerializeField]
        private float _range = 5f;
        [SerializeField]
        private int _minExpMount = 1;
        [SerializeField]
        private int _maxExpAmount = 100;

        private void Start()
        {
            for (int i = 0; i < _count; i++)
            {
                Vector3 position = transform.position + new Vector3(UnityEngine.Random.Range(-_range, _range), UnityEngine.Random.Range(-_range, _range), 0);
                int expValue = UnityEngine.Random.Range(_minExpMount, _maxExpAmount);
                _droppedGemPool.CreateDroppedGem(PlayerController.Instance, position, expValue);
            }
        }
    }
}