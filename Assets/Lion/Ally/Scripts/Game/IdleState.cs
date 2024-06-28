using Lion.CameraUtility;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public class IdleState : IState
    {
        private string _idleAnimation = "Idle";

        private float _elapsed = 0f;

        private float _changeTime = 0f;
        private float _minChangeTime = 1f;
        private float _maxChangeTime = 5f;

        private float _attackStateTransitionProbability = 0.5f;

        public void Enter(AllyController minion)
        {
            minion.Animator.Play(_idleAnimation);
            _elapsed = 0f;
            _changeTime = UnityEngine.Random.Range(_minChangeTime, _maxChangeTime);
        }

        public void Update(AllyController minion)
        {
            _elapsed += Time.deltaTime;
            // 一定時間経過したら確率に応じて、PatrolStateかAttackStateに遷移する。
            if (_elapsed > _changeTime)
            {
                ChangeStateBasedOnProbability(minion);
            }

            // カメラの範囲外になった場合、ReturnStateに遷移する。
            if (Camera.main.IsFarFromCamera(minion.transform.position))
            {
                minion.SetState<ReturnState>();
                return;
            }
        }

        public void Exit(AllyController minion)
        {

        }

        private void ChangeStateBasedOnProbability(AllyController minion)
        {
            if (UnityEngine.Random.Range(0f, 1f) < _attackStateTransitionProbability)
            {
                minion.SetState<AttackState>();
            }
            else
            {
                minion.SetState<PatrolState>();
            }
        }
    }
}