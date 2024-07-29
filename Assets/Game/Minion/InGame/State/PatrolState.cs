using Lion.Actor;
using Lion.Stage;
using System;
using UnityEngine;

namespace Lion.Minion.States
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

        public void Enter(MinionController minion)
        {
            _stagnantElapsed = 0f;
            _patrolElapsed = 0f;

            _patrolTimeThreshold = UnityEngine.Random.Range(5f, 9f);
            _stagnantTimeThreshold = UnityEngine.Random.Range(2f, 4f);

            _previousPosition = minion.transform.position;

            minion.Animator.Play(_runAnimation);
            _destination = ActivityArea.Instance.GetRandomPosition();
            var direction = _destination - minion.transform.position;
            UpdateDirection(minion, direction);
        }

        private static void UpdateDirection(MinionController minion, Vector3 direction)
        {
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
            UpdateDestination(minion);

            if (MoveTowardsDestination(minion))
            {
                ChangeStateBasedOnProbability(minion);
                return;
            }

            if (ActivityArea.Instance.IsFarFromArea(minion.transform.position))
            {
                minion.SetState<ReturnState>();
                return;
            }

            _previousPosition = minion.transform.position;
        }

        public void Exit(MinionController minion)
        {
            minion.Rigidbody2D.velocity = Vector2.zero;
        }

        private void ChangeStateBasedOnProbability(MinionController minion)
        {
            if (StageManager.Instance.IsBattleScene &&
                UnityEngine.Random.Range(0f, 1f) < _attackStateTransitionProbability)
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
            _patrolElapsed += Time.deltaTime;
            var currentPosition = minion.transform.position;
            var direction = (_destination - currentPosition).normalized;
            minion.Rigidbody2D.velocity = direction * (1.4f + minion.Status.Speed * 0.02f);

            if (_patrolElapsed > _patrolTimeThreshold)
            {
                return true;
            }

            return Vector2.SqrMagnitude(currentPosition - _destination) < 0.01f;
        }

        private void UpdateDestination(MinionController minion)
        {
            _stagnantElapsed += Time.deltaTime;
            if (_stagnantElapsed > _stagnantTimeThreshold)
            {
                _destination = ActivityArea.Instance.GetRandomPosition();
                UpdateDirection(minion, _destination - minion.transform.position);
                _stagnantElapsed = 0f;
            }
        }
    }
}