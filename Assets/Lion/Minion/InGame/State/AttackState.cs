using UnityEngine;

namespace Lion.Minion.States
{
    public class AttackState : IState
    {
        private string _attackAnimation = "Attack";

        public void Enter(MinionController minion)
        {
            minion.Animator.Play(_attackAnimation);
        }

        public void Update(MinionController minion)
        {
            minion.Rigidbody2D.velocity = Vector2.zero;
            if (minion.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                minion.SetState<IdleState>();
            }
        }

        public void Exit(MinionController minion)
        {

        }
    }
}