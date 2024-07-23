using System;
using UnityEngine;

namespace Lion.Enemy.Boss
{
    /// <summary>
    /// 近距離攻撃ステート。
    /// </summary>
    public class MeleeAttackState : Stage1BossController.IState
    {
        private readonly string _meleeAttackAnimationName = "Melee Attack";

        public void Enter(Stage1BossController boss)
        {
            boss.Animator.Play(_meleeAttackAnimationName);
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