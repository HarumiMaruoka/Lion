using System;
using System.Collections;
using UnityEngine;

namespace Character.Skill
{
    // 一定時間 自分の放つ攻撃がやけどを与える効果を得る。
    public class SampleSkill3 : ISkillBehavior
    {
        private float _duration = 1.0f;
        private bool _isStopRequest = false;

        public void Play(ISkillUser skillUser)
        {
            // スキル使用者にバフを付与する。
            skillUser.IndividualData.ApplyBurnAbility();
        }

        public IEnumerator ExecuteAsync(ISkillUser skillUser)
        {
            // 効果時間が経過するまで待機する。
            for (float t = 0f; t < _duration && !_isStopRequest; t += Time.deltaTime)
            {
                yield return null;
            }
        }

        public void Stop(ISkillUser skillUser)
        {
            // ExecuteAsyncで待機する処理をやめさせる。
            _isStopRequest = true;
        }

        public void End(ISkillUser skillUser)
        {
            // 終了処理。
            // 武器に与えたバフを取り除く。
            skillUser.IndividualData.LiftBurnAbility();
            // 停止用フラグを元に戻しておく。
            _isStopRequest = false;
        }
    }
}