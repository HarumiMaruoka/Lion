using UnityEngine;

namespace Lion.Minion.States
{
    public class IdleState : IState
    {
        private string _idleAnimation = "Idle";

        private float _elapsed = 0f;

        private float _changeTime = 0f;
        private float _minChangeTime = 1f;
        private float _maxChangeTime = 5f;

        private float _attackStateTransitionProbability = 0.5f;

        public void Enter(MinionController minion)
        {
            minion.Animator.Play(_idleAnimation);
            _elapsed = 0f;
            _changeTime = UnityEngine.Random.Range(_minChangeTime, _maxChangeTime);
        }

        public void Update(MinionController minion)
        {
            minion.Rigidbody2D.velocity = Vector2.zero;

            _elapsed += Time.deltaTime;
            // 一定時間経過したら確率に応じて、PatrolStateかAttackStateに遷移する。
            if (_elapsed > _changeTime)
            {
                ChangeStateBasedOnProbability(minion);
            }

            // プレイヤーとの距離が離れていれば、ReturnStateに遷移する。
            if (minion.IsFarFromPlayer())
            {
                minion.SetState<ReturnState>();
                return;
            }
        }

        public void Exit(MinionController minion)
        {

        }

        private void ChangeStateBasedOnProbability(MinionController minion)
        {
            if (Random.Range(0f, 1f) < _attackStateTransitionProbability)
            {
                minion.SetState<AttackState>();
            }
            else
            {
                minion.SetState<PatrolState>();
            }
        }
    }
}