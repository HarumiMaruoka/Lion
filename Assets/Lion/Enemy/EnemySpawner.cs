using Lion.CameraUtility;
using System;
using UnityEngine;

namespace Lion.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private SpawnableEnemyData[] _enemySpawnerData;

        [SerializeField] private AnimationCurve _spawnRate;

        private Vector2 TopRight => Camera.main.GetWorldTopRight() + new Vector2(3f, 3f);
        private Vector2 BottomLeft => Camera.main.GetWorldBottomLeft() - new Vector2(3f, 3f);

        private float _currentEvaluate = 0f;

        private float _intervalTimer = 0f;
        private float _interval = 0.3f;

        private void Start()
        {
            if (_spawnRate.length == 0)
            {
                Debug.LogError("SpawnRate is empty");
            }
        }

        private void Update()
        {
            _currentEvaluate += Time.deltaTime;
            _intervalTimer += Time.deltaTime;
            if (_currentEvaluate > _spawnRate[_spawnRate.length - 1].time) _currentEvaluate = 0f;

            if (_intervalTimer > _interval)
            {
                _intervalTimer = 0f;
                var evaluate = _spawnRate.Evaluate(_currentEvaluate);
                int spawnCount = (int)(evaluate);

                for (var i = 0; i < spawnCount; i++)
                {
                    SpawnEnemy();
                }
            }
        }

        private void SpawnEnemy()
        {
            var enemy = EnemyManager.Instance.EnemyPool.GetEnemy(GetRandomEnemyID());
            if (enemy == null) return;
            enemy.transform.position = GetRandomPosition();
        }

        private int GetRandomEnemyID()
        {
            var sumRate = 0f;
            foreach (var data in _enemySpawnerData)
            {
                sumRate += data.SpawnRate;
            }

            var randomRate = UnityEngine.Random.Range(0f, sumRate);
            var currentRate = 0f;
            for (var i = 0; i < _enemySpawnerData.Length; i++)
            {
                currentRate += _enemySpawnerData[i].SpawnRate;
                if (randomRate <= currentRate)
                {
                    return _enemySpawnerData[i].EnemyID;
                }
            }
            return _enemySpawnerData[_enemySpawnerData.Length - 1].EnemyID;
        }

        private Vector3 GetRandomPosition()
        {
            // 上辺、下辺、左辺、右辺のどれかにランダムに出現
            var random = UnityEngine.Random.Range(0, 4);
            switch (random)
            {
                case 0: // 上辺
                    return new Vector3(UnityEngine.Random.Range(BottomLeft.x, TopRight.x), TopRight.y, 0f);
                case 1: // 下辺
                    return new Vector3(UnityEngine.Random.Range(BottomLeft.x, TopRight.x), BottomLeft.y, 0f);
                case 2: // 左辺
                    return new Vector3(BottomLeft.x, UnityEngine.Random.Range(BottomLeft.y, TopRight.y), 0f);
                case 3: // 右辺
                    return new Vector3(TopRight.x, UnityEngine.Random.Range(BottomLeft.y, TopRight.y), 0f);
                default: // ここには来ない
                    return Vector3.zero;
            }

        }
    }

    [Serializable]
    public struct SpawnableEnemyData
    {
        public int EnemyID;
        public float SpawnRate;
    }
}
