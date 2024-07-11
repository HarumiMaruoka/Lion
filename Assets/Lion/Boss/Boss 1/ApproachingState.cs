using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Enemy.Boss
{
    /// <summary>
    /// プレイヤーに近づくステート。
    /// </summary>
    public class ApproachingState : Stage1BossController.IState
    {
        private string _runAnimationName = "Run";

        private Vector3 _targetPosition;

        public void Enter(Stage1BossController boss)
        {
            _targetPosition = PlayerController.Instance.transform.position;
            boss.Animator.Play(_runAnimationName);
        }

        public void Update(Stage1BossController boss)
        {
            // プレイヤーに向かって移動
            var moveSpeed = boss.Parameters.ApproachingState.MoveSpeed;
            boss.Rigidbody2D.velocity = (_targetPosition - boss.transform.position).normalized * moveSpeed;

            // プレイヤーとの距離を計算
            float sqrDistance = Vector2.SqrMagnitude(boss.transform.position - _targetPosition);

            // 一定距離以内に近づいたら近距離攻撃を行う
            var arrivalThresholdDistance = boss.Parameters.ApproachingState.ArrivalThresholdDistance;
            if (sqrDistance < arrivalThresholdDistance * arrivalThresholdDistance)
            {
                boss.SetState<MeleeAttackState>();
            }
        }

        public void Exit(Stage1BossController boss)
        {

        }
    }
}