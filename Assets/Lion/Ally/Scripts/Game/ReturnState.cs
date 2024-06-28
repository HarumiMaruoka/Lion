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

            // 目的地を設定する。
            _destination = Camera.main.GetRandomCameraArea();
        }

        public void Update(AllyController minion)
        {
            // 目的地に向かって移動する。
            var currentPosition = minion.transform.position;
            var direction = (_destination - currentPosition).normalized;
            minion.Rigidbody2D.velocity = direction * (1.6f + minion.Status.Speed * 0.03f);

            // プレイヤーと離れすぎている場合、強制的に目的地に移動させる。
            if (Camera.main.IsTooFarFromCamera(minion.transform.position))
            {
                minion.transform.position = _destination;
            }

            // 目的地に到達したら、IdleStateに遷移する。
            if (Vector2.SqrMagnitude(currentPosition - _destination) < 0.01f)
            {
                minion.SetState<IdleState>();
            }

            // 目的地がカメラの範囲外になった場合、目的地を再設定する。
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