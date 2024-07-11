using Cysharp.Threading.Tasks;
using Lion.Weapon.Behaviour.ElectricChargerModules;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    // エレクトリックチャージャー (Electric Charger):
    // プレイヤーの周囲に電気フィールドを発生させ、近づいてきた敵に電撃を与える。
    // フィールドは徐々に拡大し、連鎖して他の敵にもダメージを与える。
    public class ElectricCharger : WeaponBehaviour
    {
        private Func<UniTask>[] _tasks = new Func<UniTask>[2];

        [SerializeField]
        private float _minFireInterval = 0.5f;
        [SerializeField]
        private float _maxFireInterval = 2f;

        private float WaitTime
        {
            get
            {
                if (Parameter == null) return _minFireInterval;
                return Mathf.Clamp(_maxFireInterval - Parameter.AttackSpeed * 0.01f, _minFireInterval, _maxFireInterval);
            }
        }

        private void Start()
        {
            _tasks[0] = Fire;
            _tasks[1] = Wait;

            RunSequence();
        }

        private async void RunSequence()
        {
            int index = 0;
            while (this)
            {
                await _tasks[index]();
                index = (index + 1) % _tasks.Length;
            }
        }

        [SerializeField]
        private ElectricField _electricFieldPrefab;

        private async UniTask Fire()
        {
            var bullet = Instantiate(_electricFieldPrefab, transform.position, transform.rotation);
            bullet.Parameter = Parameter;

            while (bullet)
            {
                await UniTask.Yield();
            }
        }

        private async UniTask Wait()
        {
            var w = WaitTime;
            for (float t = 0f; t < w; t += Time.deltaTime)
            {
                await UniTask.Yield();
            }
        }
    }
}
