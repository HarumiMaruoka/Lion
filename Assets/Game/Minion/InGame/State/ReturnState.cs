using Lion.Actor;
using System;
using UnityEngine;

namespace Lion.Minion.States
{
    public class ReturnState : IState
    {
        private string _runAnimation = "Run";

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
            // 目的地に向かって移動する。
            var currentPosition = minion.transform.position;
            var direction = (_destination - currentPosition).normalized;
            minion.Rigidbody2D.velocity = direction * minion.Status.MoveSpeed * 1.5f;

            // プレイヤーと離れすぎている場合、強制的に目的地に移動させる。
            if (ActivityArea.Instance.IsFarFromArea(minion.transform.position, 5f))
            {
                minion.transform.position = _destination;
            }

            // 目的地に到達したら、IdleStateに遷移する。
            if (Vector2.SqrMagnitude(currentPosition - _destination) < 0.01f)
            {
                minion.SetState<IdleState>();
            }

            // 目的地がプレイヤーから遠すぎる場合、再度目的地を設定する。
            if (ActivityArea.Instance.IsFarFromArea(_destination))
            {
                _destination = ActivityArea.Instance.GetRandomPosition();
            }
        }

        public void Exit(MinionController minion)
        {
            minion.Rigidbody2D.velocity = Vector2.zero;
        }
    }
}