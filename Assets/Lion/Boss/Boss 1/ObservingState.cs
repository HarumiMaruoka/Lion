using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Enemy.Boss
{
    /// <summary>
    /// 積極的に攻撃するのではなく、距離を保ちながらプレイヤーの動きを観察する状態。
    /// </summary>
    public class ObservingState : Stage1BossController.IState
    {
        // プレイヤーが近づけば離れる。
        // プレイヤーが離れれば近づく。

        // プレイヤーがとても近くにいる場合、近距離攻撃を行う。
        // プレイヤーが遠くにいる場合、遠距離攻撃を行う。

        // 一定時間経過した後、次の行動を決定する。
        private Stage1BossParameters.ObservingStateParameters Parameters;
        private Transform Player => PlayerController.Instance.transform;

        public void Enter(Stage1BossController boss)
        {
            Parameters = boss.Parameters.ObservingState;
            ObservingTime = UnityEngine.Random.Range(ObservingMinTime, ObservingMaxTime);
        }

        public void Update(Stage1BossController boss)
        {
            UpdateObservingTime(boss);
            UpdateMeleeAttack(boss);
            UpdateMaintainDistance(boss);
            UpdateRangeAttack(boss);
        }


        public void Exit(Stage1BossController boss)
        {

        }

        #region ObservingTime
        private float ObservingMinTime => Parameters.ObservingMinTime;
        private float ObservingMaxTime => Parameters.ObservingMaxTime;

        private float ObservingTime;

        private void UpdateObservingTime(Stage1BossController boss)
        {
            ObservingTime -= Time.deltaTime;

            if (ObservingTime <= 0)
            {
                // ここに次のステートへの遷移処理を書く。
                // テスト
                boss.SetState<IdleState>();
            }
        }
        #endregion

        #region MeleeAttack
        private float ArrivalThresholdDistance => Parameters.ArrivalThresholdDistance;

        private void UpdateMeleeAttack(Stage1BossController boss)
        {
            // プレイヤーとの距離を計算
            float sqrDistance = Vector2.SqrMagnitude(boss.transform.position - Player.position);

            // 一定距離以内に近づいたら近距離攻撃を行う
            if (sqrDistance < ArrivalThresholdDistance * ArrivalThresholdDistance)
            {
                boss.SetState<MeleeAttackState>();
            }
        }
        #endregion

        #region MaintainDistance
        private float ObservingMaxDistance => Parameters.ObservingMaxDistance;
        private float ObservingMinDistance => Parameters.ObservingMinDistance;
        private float MoveSpeed => Parameters.MoveSpeed;

        private bool _isIdle;

        private void UpdateMaintainDistance(Stage1BossController boss)
        {
            // プレイヤーとの距離を計算
            float sqrDistance = Vector2.SqrMagnitude(boss.transform.position - Player.position);

            // プレイヤーとの距離が一定範囲内に収まるように移動
            if (sqrDistance > ObservingMaxDistance * ObservingMaxDistance)
            {
                if (_isIdle)
                {
                    boss.Animator.Play("Run");
                    _isIdle = false;
                }
                boss.Rigidbody2D.velocity = (Player.position - boss.transform.position).normalized * MoveSpeed;
            }
            else if (sqrDistance < ObservingMinDistance * ObservingMinDistance)
            {
                if (_isIdle)
                {
                    boss.Animator.Play("Run");
                    _isIdle = false;
                }
                boss.Rigidbody2D.velocity = (boss.transform.position - Player.position).normalized * MoveSpeed;
            }
            else
            {
                if (!_isIdle)
                {
                    boss.Animator.Play("Idle");
                    _isIdle = true;
                }
                boss.Rigidbody2D.velocity = Vector2.zero;
            }
        }
        #endregion

        #region RangeAttack
        private float RangeAttackThresholdDistance => Parameters.RangeAttackThresholdDistance;

        private void UpdateRangeAttack(Stage1BossController boss)
        {
            // プレイヤーとの距離を計算
            float sqrDistance = Vector2.SqrMagnitude(boss.transform.position - Player.position);

            // 一定距離以上離れたら遠距離攻撃を行う
            if (sqrDistance > RangeAttackThresholdDistance * RangeAttackThresholdDistance)
            {
                boss.SetState<RangeAttackState>();
            }
        }
        #endregion
    }
}