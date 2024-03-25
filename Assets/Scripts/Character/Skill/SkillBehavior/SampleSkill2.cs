using System;
using System.Collections;
using UnityEngine;

namespace Character.Skill
{
    // 周囲の敵の移動速度を減衰する。
    public class SampleSkill2 : ISkillBehavior
    {
        private float _effectRange = 10f; // 半径。

        private float _duration = 5f;
        private bool _isCancelRequested = false;

        public void Play(ISkillUser skillUser)
        {

        }

        public IEnumerator ExecuteAsync(ISkillUser skillUser)
        {
            for (float t = 0f; t < _duration && !_isCancelRequested; t += Time.deltaTime)
            {
                // スキル使用者の周囲の敵の移動速度を減衰する効果を記述する。
                foreach (var enemy in EnemyManager.Current.ActiveEnemies)
                {
                    var sqrDistance = (skillUser.Transform.position - enemy.transform.position).sqrMagnitude;
                    var sqrEffectRange = _effectRange * _effectRange;
                    if (sqrDistance < sqrEffectRange)
                    {
                        enemy.ApplyMoveSpeedEffect(0.5f);
                    }
                }

                yield return null;
            }
        }

        public void Stop(ISkillUser skillUser)
        {
            _isCancelRequested = true;
        }

        public void End(ISkillUser skillUser)
        {
            _isCancelRequested = false;
        }
    }
}