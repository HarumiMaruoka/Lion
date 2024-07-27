using Lion.Actor;
using Lion.CameraUtility;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public class PatrolState : IState
    {
        private string _runAnimation = "Run";

        private float _attackStateTransitionProbability = 0.5f;

        private Vector3 _destination;

        public void Enter(AllyController ally)
        {
            ally.Animator.Play(_runAnimation);
            // 目的地を設定する。
            _destination = ActivityArea.Instance.GetRandomPosition();
            // 向きを設定する。
            var direction = _destination - ally.transform.position;
            if (direction.x > 0 && ally.transform.localScale.x < 0)
            {
                ally.transform.localScale = new Vector3(Mathf.Abs(ally.transform.localScale.x), ally.transform.localScale.y, 1);
            }
            else if (direction.x < 0 && ally.transform.localScale.x > 0)
            {
                ally.transform.localScale = new Vector3(-Mathf.Abs(ally.transform.localScale.x), ally.transform.localScale.y, 1);
            }
        }

        public void Update(AllyController ally)
        {
            // 移動し目的地に到達したら、確率に応じてIdleStateかAttackStateに遷移する。
            if (MoveTowardsDestination(ally))
            {
                ChangeStateBasedOnProbability(ally);
                return;
            }

            // ActivityAreaから離れたらReturnStateに遷移する。
            if (ActivityArea.Instance.IsFarFromArea(ally.transform.position))
            {
                ally.SetState<ReturnState>();
                return;
            }
        }

        public void Exit(AllyController ally)
        {
            ally.Rigidbody2D.velocity = Vector2.zero;
        }

        private void ChangeStateBasedOnProbability(AllyController ally)
        {
            if (UnityEngine.Random.Range(0f, 1f) < _attackStateTransitionProbability)
            {
                ally.SetState<AttackState>();
            }
            else
            {
                ally.SetState<IdleState>();
            }
        }

        private bool MoveTowardsDestination(AllyController ally)
        {
            var currentPosition = ally.transform.position;
            var direction = (_destination - currentPosition).normalized;
            ally.Rigidbody2D.velocity = direction * (1.4f + ally.Status.Speed * 0.02f);

            return Vector2.SqrMagnitude(currentPosition - _destination) < 0.01f;
        }
    }
}
