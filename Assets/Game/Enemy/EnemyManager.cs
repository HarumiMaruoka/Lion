using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lion.Enemy
{
    public class EnemyManager
    {
        public static EnemyManager Instance { get; private set; } = new EnemyManager();

        public EnemySheet EnemySheet { get; private set; }
        public EnemyPool EnemyPool { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Instance.EnemySheet = ScriptableObject.Instantiate(Resources.Load<EnemySheet>("EnemySheet"));
            Instance.EnemySheet.Initialize();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public static bool TryGetEnemy(GameObject gameObject, out EnemyController enemy)
        {
            return Instance.EnemyPool.TryGetEnemy(gameObject, out enemy);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            Instance.EnemyPool = new EnemyPool(Instance.EnemySheet);
        }
    }
}