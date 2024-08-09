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
            UpdateState(boss);
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

        private enum Direction
        {
            Approach, // 近づく
            Retreat, // 離れる
            Idle, // 待機
        }

        private Direction _currentState;
        private float _switchTimer;
        private float _switchInterval = 0.5f;
        private bool IsSwitchable => _switchTimer <= 0;

        private void UpdateMaintainDistance(Stage1BossController boss)
        {
            _switchTimer -= Time.deltaTime;
            switch (_currentState)
            {
                case Direction.Approach:
                    boss.Rigidbody2D.velocity = (Player.position - boss.transform.position).normalized * MoveSpeed;
                    break;
                case Direction.Retreat:
                    boss.Rigidbody2D.velocity = (boss.transform.position - Player.position).normalized * MoveSpeed;
                    break;
                case Direction.Idle:
                    boss.Rigidbody2D.velocity = Vector2.zero;
                    break;
                default:
                    break;
            }
        }

        private void UpdateState(Stage1BossController boss)
        {
            // プレイヤーとの距離を計算
            float sqrDistance = Vector2.SqrMagnitude(boss.transform.position - Player.position);

            // プレイヤーとの距離が一定範囲内に収まるように移動
            if (_currentState != Direction.Approach && sqrDistance > ObservingMaxDistance * ObservingMaxDistance && IsSwitchable)
            {
                boss.Animator.Play("Run");
                _currentState = Direction.Approach;
                _switchTimer = _switchInterval;
            }
            else if (_currentState != Direction.Retreat && sqrDistance < ObservingMinDistance * ObservingMinDistance && IsSwitchable)
            {
                boss.Animator.Play("Run");
                _currentState = Direction.Retreat;
                _switchTimer = _switchInterval;
            }
            else if (_currentState != Direction.Idle && IsSwitchable)
            {
                boss.Animator.Play("Idle");
                _currentState = Direction.Idle;
                _switchTimer = _switchInterval;
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