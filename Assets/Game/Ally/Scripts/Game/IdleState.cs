using Lion.Actor;
using Lion.CameraUtility;
using Lion.Stage;
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

        public void Enter(AllyController ally)
        {
            ally.Animator.Play(_idleAnimation);
            _elapsed = 0f;
            _changeTime = UnityEngine.Random.Range(_minChangeTime, _maxChangeTime);
        }

        public void Update(AllyController ally)
        {
            ally.Rigidbody2D.velocity = Vector2.zero;

            _elapsed += Time.deltaTime;
            // 一定時間経過したら確率に応じて、PatrolStateかAttackStateに遷移する。
            if (_elapsed > _changeTime)
            {
                ChangeStateBasedOnProbability(ally);
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
                ally.SetState<PatrolState>();
            }
        }
    }
}