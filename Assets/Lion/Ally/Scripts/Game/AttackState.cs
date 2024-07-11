using System;
using UnityEngine;

namespace Lion.Ally
{
    public class AttackState : IState
    {
        private string _attackAnimation = "Attack";

        public void Enter(AllyController ally)
        {
            ally.Animator.Play(_attackAnimation);
        }

        public void Update(AllyController ally)
        {
            ally.Rigidbody2D.velocity = Vector2.zero;

            if (ally.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                ally.SetState<IdleState>();
            }
        }

        public void Exit(AllyController ally)
        {

        }
    }
}
