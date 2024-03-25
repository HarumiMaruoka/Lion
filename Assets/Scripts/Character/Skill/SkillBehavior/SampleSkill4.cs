using System;
using System.Collections;
using UnityEngine;

namespace Character.Skill
{
    // 全ての敵を混乱にする。
    public class SampleSkill4 : ISkillBehavior
    {
        private float _duration = 10f;

        public void Play(ISkillUser skillUser)
        {
            // 現在 存在する全ての敵に混乱を付与する。
            foreach (var enemy in EnemyManager.Current.ActiveEnemies)
            {
                enemy.ApplyConfusion(_duration);
            }
        }

        public IEnumerator ExecuteAsync(ISkillUser skillUser)
        {
            // 特に何かする必要はない。
            yield break;
        }

        public void Stop(ISkillUser skillUser)
        {
            // 特に何かする必要はない。
        }

        public void End(ISkillUser skillUser)
        {
            // 特に何かする必要はない。
        }
    }
}