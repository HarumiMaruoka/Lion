using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Gem
{
    public class DroppedGemPool : MonoBehaviour
    {
        [SerializeField]
        private DroppedGem _prefab = default;

        private readonly HashSet<DroppedGem> _activePool = new HashSet<DroppedGem>();
        private readonly Queue<DroppedGem> _inactivePool = new Queue<DroppedGem>();

        public DroppedGem CreateDroppedGem(Vector3 position, int amount)
        {
            DroppedGem gem;
            if (_inactivePool.Count == 0)
            {
                gem = Instantiate(_prefab, position, Quaternion.identity, transform);
                gem.Amount = amount;
                gem.Pool = this;
            }
            else
            {
                gem = _inactivePool.Dequeue();
                gem.transform.position = position;
                gem.Amount = amount;
                gem.gameObject.SetActive(true);
            }
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