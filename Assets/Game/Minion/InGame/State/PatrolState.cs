using Lion.Actor;
using UnityEngine;

namespace Lion.Minion.States
{
    public class PatrolState : IState
    {
        private string _runAnimation = "Run";

        private float _attackStateTransitionProbability = 0.5f;

        private Vector3 _destination;

        public void Enter(MinionController minion)
        {
            minion.Animator.Play(_runAnimation);
            // 目的地を設定する。
            _destination = ActivityArea.Instance.GetRandomPosition();

            // 向きを設定する。
            var direction = _destination - minion.transform.position;
            if (direction.x > 0 && minion.transform.localScale.x < 0)
            {
                minion.transform.localScale = new Vector3(Mathf.Abs(minion.transform.localScale.x), minion.transform.localScale.y, 1);
            }
            else if (direction.x < 0 && minion.transform.localScale.x > 0)
            {
                minion.transform.localScale = new Vector3(-Mathf.Abs(minion.transform.localScale.x), minion.transform.localScale.y, 1);
            }
        }

        public void Update(MinionController minion)
        {
            // 移動し目的地に到達したら、確率に応じてIdleStateかAttackStateに遷移する。
            if (MoveTowardsDestination(minion))
            {
                ChangeStateBasedOnProbability(minion);
                return;
            }

            // ActivityAreaから離れたらReturnStateに遷移する。
            if (ActivityArea.Instance.IsFarFromArea(minion.transform.position))
            {
                minion.SetState<ReturnState>();
                return;
            }
        }

        public void Exit(MinionController servantDemon)
        {
            servantDemon.Rigidbody2D.velocity = Vector2.zero;
        }

        private void ChangeStateBasedOnProbability(MinionController minion)
        {
            if (Random.Range(0f, 1f) < _attackStateTransitionProbability)
            {
                minion.SetState<AttackState>();
            }
            else
            {
                minion.SetState<IdleState>();
            }
        }

        private bool MoveTowardsDestination(MinionController minion)
        {
            var currentPosition = minion.transform.position;
            var direction = (_destination - currentPosition).normalized;
            minion.Rigidbody2D.velocity = direction * minion.Status.MoveSpeed;

            return Vector2.SqrMagnitude(currentPosition - _destination) < 0.01f;
        }
    }
}