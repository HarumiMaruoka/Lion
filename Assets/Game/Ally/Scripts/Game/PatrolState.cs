using Lion.Actor;
using Lion.Stage;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public class PatrolState : IState
    {
        private string _runAnimation = "Run";
        private float _attackStateTransitionProbability = 0.5f;
        private Vector3 _destination;
        private Vector3 _previousPosition;
        private float _patrolElapsed = 0f;
        private float _patrolTimeThreshold = 8f;
        private float _stagnantElapsed = 0f;
        private float _stagnantTimeThreshold = 3f;

        public void Enter(AllyController ally)
        {
            _stagnantElapsed = 0f;
            _patrolElapsed = 0f;

            _patrolTimeThreshold = UnityEngine.Random.Range(5f, 9f);
            _stagnantTimeThreshold = UnityEngine.Random.Range(2f, 4f);

            _previousPosition = ally.transform.position;

            ally.Animator.Play(_runAnimation);
            _destination = ActivityArea.Instance.GetRandomPosition();
            var direction = _destination - ally.transform.position;
            UpdateDirection(ally, direction);
        }

        private static void UpdateDirection(AllyController ally, Vector3 direction)
        {
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
            UpdateDestination(ally);

            if (MoveTowardsDestination(ally))
            {
                ChangeStateBasedOnProbability(ally);
                return;
            }

            if (ActivityArea.Instance.IsFarFromArea(ally.transform.position))
            {
                ally.SetState<ReturnState>();
                return;
            }

            _previousPosition = ally.transform.position;
        }

        public void Exit(AllyController ally)
        {
            ally.Rigidbody2D.velocity = Vector2.zero;
        }

        private void ChangeStateBasedOnProbability(AllyController ally)
        {
            if (StageManager.Instance.IsBattleScene &&
                UnityEngine.Random.Range(0f, 1f) < _attackStateTransitionProbability)
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
            _patrolElapsed += Time.deltaTime;
            var currentPosition = ally.transform.position;
            var direction = (_destination - currentPosition).normalized;
            ally.Rigidbody2D.velocity = direction * (1.4f + ally.Status.Speed * 0.02f);

            if (_patrolElapsed > _patrolTimeThreshold)
            {
                return true;
            }

            return Vector2.SqrMagnitude(currentPosition - _destination) < 0.01f;
        }

        private void UpdateDestination(AllyController ally)
        {
            _stagnantElapsed += Time.deltaTime;
            if (_stagnantElapsed > _stagnantTimeThreshold)
            {
                _destination = ActivityArea.Instance.GetRandomPosition();
                UpdateDirection(ally, _destination - ally.transform.position);
                _stagnantElapsed = 0f;
            }
        }
    }
}