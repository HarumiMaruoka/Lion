using System;
using System.Collections;
using UnityEngine;

namespace Character
{
    public interface ISkillBehavior
    {
        public void Play(ISkillUser skillUser); // 効果発動命令。

        public IEnumerator ExecuteAsync(ISkillUser skillUser); // 効果発動中 毎フレーム呼ばれる。

        public void Stop(ISkillUser skillUser); // 停止命令。

        public void End(ISkillUser skillUser); // 効果が切れた時に呼ばれる。
    }
}