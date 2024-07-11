using System;
using UnityEngine;

namespace Lion.Enemy.Boss
{
    /// <summary>
    /// 遠距離攻撃ステート。
    /// </summary>
    public class RangeAttackState : Stage1BossController.IState
    {
        private readonly string _rangeAttackAnimationName = "Range Attack";

        public void Enter(Stage1BossController boss)
        {
            boss.Animator.Play(_rangeAttackAnimationName);
        }

        public void Update(Stage1BossController boss)
        {
            boss.Rigidbody2D.velocity = Vector2.zero;

            if (boss.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                // ここに次のステートへの遷移処理を書く。
                // テスト
                boss.SetState<IdleState>();
            }
        }

        public void Exit(Stage1BossController boss)
        {

        }
    }
}