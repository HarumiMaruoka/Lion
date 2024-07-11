using System;
using UnityEngine;

namespace Lion.Enemy.Boss
{
    /// <summary>
    /// 待機ステート。
    /// </summary>
    public class IdleState : Stage1BossController.IState
    {
        private string _idleAnimationName = "Idle";

        // ランダム時間待機して、次の行動を決定する。
        private float _waitMinTime;
        private float _waitMaxTime;

        private float _idleTime;

        public void Enter(Stage1BossController boss)
        {
            boss.Animator.Play(_idleAnimationName);
            _idleTime = UnityEngine.Random.Range(_waitMinTime, _waitMaxTime);
        }

        public void Update(Stage1BossController boss)
        {
            boss.Rigidbody2D.velocity = Vector2.zero;

            _idleTime -= Time.deltaTime;

            if (_idleTime < 0)
            {
                // ここに次のステートへの遷移処理を書く。
                // テスト用にランダムで次のステートを決定する。
                var rand = UnityEngine.Random.Range(0, 3);
                if (rand == 0)
                {
                    boss.SetState<ApproachingState>();
                }
                else if (rand == 1)
                {
                    boss.SetState<RetreatState>();
                }
                else
                {
                    boss.SetState<ObservingState>();
                }
            }
        }

        public void Exit(Stage1BossController boss)
        {

        }
    }
}