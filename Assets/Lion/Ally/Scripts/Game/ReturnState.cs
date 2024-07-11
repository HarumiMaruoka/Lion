using Lion.CameraUtility;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public class ReturnState : IState
    {
        private string _runAnimation = "Run";

        private Vector3 _destination;

        public void Enter(AllyController ally)
        {
            ally.Animator.Play(_runAnimation);

            // 目的地を設定する。
            _destination = Camera.main.GetRandomCameraArea();
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
            // 目的地に向かって移動する。
            var currentPosition = ally.transform.position;
            var direction = (_destination - currentPosition).normalized;
            ally.Rigidbody2D.velocity = direction * (1.6f + ally.Status.Speed * 0.03f);

            // プレイヤーと離れすぎている場合、強制的に目的地に移動させる。
            if (Camera.main.IsTooFarFromCamera(ally.transform.position))
            {
                ally.transform.position = _destination;
            }

            // 目的地に到達したら、IdleStateに遷移する。
            if (Vector2.SqrMagnitude(currentPosition - _destination) < 0.01f)
            {
                ally.SetState<IdleState>();
            }

            // 目的地がカメラの範囲外になった場合、目的地を再設定する。
            if (Camera.main.IsFarFromCamera(_destination))
            {
                _destination = Camera.main.GetRandomCameraArea();
            }
        }

        public void Exit(AllyController ally)
        {
            ally.Rigidbody2D.velocity = Vector2.zero;
        }
    }
}