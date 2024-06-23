using System.Collections.Generic;
using UnityEngine;

namespace Lion.Gold
{
    public class DroppedGoldPool : MonoBehaviour
    {
        [SerializeField]
        private DroppedGold _prefab = default;

        private readonly HashSet<DroppedGold> _activePool = new HashSet<DroppedGold>();
        private readonly Queue<DroppedGold> _inactivePool = new Queue<DroppedGold>();

        public DroppedGold CreateDroppedGold(Vector3 position, int amount)
        {
            DroppedGold gold;
            if (_inactivePool.Count == 0)
            {
                gold = Instantiate(_prefab, position, Quaternion.identity, transform);
                gold.Amount = amount;
                gold.Pool = this;
            }
            else
            {
                gold = _inactivePool.Dequeue();
                gold.transform.position = position;
                gold.Amount = amount;
                gold.gameObject.SetActive(true);
            }
            _activePool.Add(gold);
            return gold;
        }

        public void Deactivate(DroppedGold droppedGold)
        {
            if (_activePool.Remove(droppedGold))
            {
                _inactivePool.Enqueue(droppedGold);
                droppedGold.gameObject.SetActive(false);
            }
        }
    }
}