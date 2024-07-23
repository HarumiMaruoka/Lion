using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lion.Gem
{
    public class DroppedGemPool : MonoBehaviour
    {
        public static DroppedGemPool Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("DroppedGemPool is already exist.");
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        [SerializeField]
        private DroppedGem _prefab = default;

        private readonly HashSet<DroppedGem> _activePool = new HashSet<DroppedGem>();
        private readonly Queue<DroppedGem> _inactivePool = new Queue<DroppedGem>();

        public int ActiveCount => _activePool.Count;

        public DroppedGem CreateDroppedGem(IGemCollector collector, Vector3 position, int amount)
        {
            DroppedGem gem;
            if (_inactivePool.Count == 0)
            {
                gem = Instantiate(_prefab, position, Quaternion.identity, transform);
                gem.Pool = this;
            }
            else
            {
                gem = _inactivePool.Dequeue();
                gem.gameObject.SetActive(true);
            }
            gem.Initialize(collector, position, amount);

            _activePool.Add(gem);
            return gem;
        }

        public void Deactivate(DroppedGem droppedGem)
        {
            if (_activePool.Remove(droppedGem))
            {
                _inactivePool.Enqueue(droppedGem);
                droppedGem.gameObject.SetActive(false);
            }
        }
    }
}