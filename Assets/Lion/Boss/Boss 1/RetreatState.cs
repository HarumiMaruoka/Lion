using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Enemy.Boss
{
    /// <summary>
    /// プレイヤーから離れるステート。
    /// </summary>
    public class RetreatState : Stage1BossController.IState
    {
        private readonly string _runAnimationName = "Run";

        private Vector3 _targetPosition;

        public void Enter(Stage1BossController boss)
        {
            _targetPosition = PlayerController.Instance.transform.position;
            boss.Animator.Play(_runAnimationName);
        }

        public void Update(Stage1BossController boss)
        {
            // プレイヤーから離れるように移動
            var moveSpeed = boss.Parameters.RetreatState.MoveSpeed;
            boss.Rigidbody2D.velocity = (boss.transform.position - _targetPosition).normalized * moveSpeed;

            // プレイヤーとの距離を計算
            float sqrDistance = Vector2.SqrMagnitude(boss.transform.position - _targetPosition);

            // 一定距離以上離れたら遠距離攻撃を行う
            var arrivalThresholdDistance = boss.Parameters.RetreatState.ArrivalThresholdDistance;
            if (sqrDistance > arrivalThresholdDistance * arrivalThresholdDistance)
            {
                boss.SetState<RangeAttackState>();
            }
        }

        public void Exit(Stage1BossController boss)
        {

        }
    }
}