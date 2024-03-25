using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Skill
{
    // 存在する敵を全て滅ぼす。
    public class SampleSkill1 : ISkillBehavior
    {
        private List<EnemyController> _enemies = new List<EnemyController>();

        public void Play(ISkillUser skillUser)
        {
            _enemies.Clear();
            _enemies.AddRange(EnemyManager.Current.ActiveEnemies);

            foreach (var enemy in _enemies)
            {
                enemy.Kill();
            }
        }

        public IEnumerator ExecuteAsync(ISkillUser skillUser)
        {
            // 特になにかする必要はない。
            yield break;
        }

        public void Stop(ISkillUser skillUser)
        {
            // 特になにかする必要はない。
        }

        public void End(ISkillUser skillUser)
        {
            // 特になにかする必要はない。
        }
    }
}