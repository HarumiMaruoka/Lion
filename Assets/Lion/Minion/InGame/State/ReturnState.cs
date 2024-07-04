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
            _destination = minion.GetRandomPositionNearPlayer();
        }

        public void Update(MinionController minion)
        {
            // 目的地に向かって移動する。
            var currentPosition = minion.transform.position;
            var direction = (_destination - currentPosition).normalized;
            minion.Rigidbody2D.velocity = direction * (1.6f + minion.Status.Speed * 0.03f);

            // プレイヤーと離れすぎている場合、強制的に目的地に移動させる。
            if (minion.IsTooFarFromPlayer())
            {
                minion.transform.position = _destination;
            }

            // 目的地に到達したら、IdleStateに遷移する。
            if (Vector2.SqrMagnitude(currentPosition - _destination) < 0.01f)
            {
                minion.SetState<IdleState>();
            }

            // 目的地がプレイヤーから遠すぎる場合、再度目的地を設定する。
            if (TargetIsFarFromPlayer(minion))
            {
                _destination = minion.GetRandomPositionNearPlayer();
            }
        }

        public void Exit(MinionController minion)
        {
            minion.Rigidbody2D.velocity = Vector2.zero;
        }

        private bool TargetIsFarFromPlayer(MinionController minion)
        {
            var distanceX = (minion.BottomRight.x - minion.TopLeft.x) / 2f;
            var distanceY = (minion.BottomRight.y - minion.TopLeft.y) / 2f;

            return minion.IsFarFromPlayer((Vector2)_destination, new Vector2(distanceX, distanceY));
        }
    }
}