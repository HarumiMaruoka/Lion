using Lion.CameraUtility;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public class ReturnState : IState
    {
        private string _runAnimation = "Run";

        private Vector3 _destination;

        public void Enter(AllyController minion)
        {
            minion.Animator.Play(_runAnimation);

            // �ړI�n��ݒ肷��B
            _destination = Camera.main.GetRandomCameraArea();
        }

        public void Update(AllyController minion)
        {
            // �ړI�n�Ɍ������Ĉړ�����B
            var currentPosition = minion.transform.position;
            var direction = (_destination - currentPosition).normalized;
            minion.Rigidbody2D.velocity = direction * (1.6f + minion.Status.Speed * 0.03f);

            // �v���C���[�Ɨ��ꂷ���Ă���ꍇ�A�����I�ɖړI�n�Ɉړ�������B
            if (Camera.main.IsTooFarFromCamera(minion.transform.position))
            {
                minion.transform.position = _destination;
            }

            // �ړI�n�ɓ��B������AIdleState�ɑJ�ڂ���B
            if (Vector2.SqrMagnitude(currentPosition - _destination) < 0.01f)
            {
                minion.SetState<IdleState>();
            }

            // �ړI�n���J�����͈̔͊O�ɂȂ����ꍇ�A�ړI�n���Đݒ肷��B
            if (Camera.main.IsFarFromCamera(_destination))
            {
                _destination = Camera.main.GetRandomCameraArea();
            }
        }

        public void Exit(AllyController minion)
        {
            minion.Rigidbody2D.velocity = Vector2.zero;
        }
    }
}