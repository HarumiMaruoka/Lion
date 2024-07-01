using System;
using UnityEngine;

namespace Lion.Ally
{
    public class ColliderToggleAttacker : AllyController
    {
        [SerializeField]
        private AttackArea _attackArea;

        protected override void Start()
        {
            base.Start();
            _attackArea.AllyController = this;
        }

        public void BeginAttack()
        {
            _attackArea.gameObject.SetActive(true);
        }

        public void EndAttack()
        {
            _attackArea.gameObject.SetActive(false);
        }
    }
}