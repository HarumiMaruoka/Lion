using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Enemy
{
    public class EnemyPool
    {
        private Dictionary<int, HashSet<EnemyController>> _activePool = new Dictionary<int, HashSet<EnemyController>>();
        private Dictionary<int, Queue<EnemyController>> _inactivePool = new Dictionary<int, Queue<EnemyController>>();

        // キーはgameObjectのインスタンスID
        private Dictionary<int, EnemyController> _activeEnemies = new Dictionary<int, EnemyController>();
        public Dictionary<int, EnemyController> ActiveEnemies => _activeEnemies;

        public EnemyController FindEnemy(int instanceID)
        {
            if (_activeEnemies.ContainsKey(instanceID))
            {
                return _activeEnemies[instanceID];
            }
            return null;
        }

        public EnemyPool(EnemySheet enemyDatas)
        {
            foreach (var data in enemyDatas)
            {
                _activePool.Add(data.ID, new HashSet<EnemyController>());
                _inactivePool.Add(data.ID, new Queue<EnemyController>());
            }
        }

        public EnemyController GetEnemy(int id)
        {
            if (!_inactivePool.ContainsKey(id))
            {
                Debug.LogError($"Invalid Enemy ID: {id}");
                return null;
            }

            if (_inactivePool[id].Count == 0)
            {
                var enemyData = EnemyManager.Instance.EnemySheet.GetEnemyData(id);
                if (enemyData == null || enemyData.Prefab == null) return null;
                var instance = GameObject.Instantiate(enemyData.Prefab);
                instance.EnemyData = enemyData;
                _inactivePool[id].Enqueue(instance);
            }

            var enemy = _inactivePool[id].Dequeue();
            enemy.gameObject.SetActive(true);
            enemy.Initialize();
            _activePool[id].Add(enemy);
            _activeEnemies.Add(enemy.gameObject.GetInstanceID(), enemy);
            return enemy;
        }

        public void ReturnEnemy(EnemyController enemy)
        {
            if (!_activePool.ContainsKey(enemy.EnemyData.ID))
            {
                Debug.LogError($"Invalid Enemy ID: {enemy.EnemyData.ID}");
                return;
            }

            enemy.gameObject.SetActive(false);
            _activePool[enemy.EnemyData.ID].Remove(enemy);
            _activeEnemies.Remove(enemy.gameObject.GetInstanceID());
            _inactivePool[enemy.EnemyData.ID].Enqueue(enemy);
        }
    }
}