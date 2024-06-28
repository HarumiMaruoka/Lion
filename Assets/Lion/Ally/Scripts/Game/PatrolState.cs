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

        public void Enter(AllyController minion)
        {
            minion.Animator.Play(_runAnimation);
            // �ړI�n��ݒ肷��B
            _destination = Camera.main.GetRandomCameraArea();
        }

        public void Update(AllyController minion)
        {
            // �ړ����ړI�n�ɓ��B������A�m���ɉ�����IdleState��AttackState�ɑJ�ڂ���B
            if (MoveTowardsDestination(minion))
            {
                ChangeStateBasedOnProbability(minion);
                return;
            }

            // �J�����͈̔͊O�ɂȂ����ꍇ�AReturnState�ɑJ�ڂ���B
            if (Camera.main.IsFarFromCamera(minion.transform.position))
            {
                minion.SetState<ReturnState>();
                return;
            }
        }

        public void Exit(AllyController servantDemon)
        {
            servantDemon.Rigidbody2D.velocity = Vector2.zero;
        }

        private void ChangeStateBasedOnProbability(AllyController minion)
        {
            if (UnityEngine.Random.Range(0f, 1f) < _attackStateTransitionProbability)
            {
                minion.SetState<AttackState>();
            }
            else
            {
                minion.SetState<IdleState>();
            }
        }

        private bool MoveTowardsDestination(AllyController minion)
        {
            var currentPosition = minion.transform.position;
            var direction = (_destination - currentPosition).normalized;
            minion.Rigidbody2D.velocity = direction * (1.4f + minion.Status.Speed * 0.02f);

            return Vector2.SqrMagnitude(currentPosition - _destination) < 0.01f;
        }
    }
}
