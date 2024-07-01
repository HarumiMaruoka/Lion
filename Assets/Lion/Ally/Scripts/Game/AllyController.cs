using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Ally
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class AllyController : MonoBehaviour
    {
        public AllyData AllyData { get; set; }
        public AllyStatus Status => AllyData == null ? default : AllyData.Status;

        public Rigidbody2D Rigidbody2D { get; private set; }
        public Animator Animator { get; private set; }
        public IState CurrentState { get; private set; }

        private Dictionary<Type, IState> _states = new Dictionary<Type, IState>()
        {
            {typeof(IdleState), new IdleState()},
            {typeof(PatrolState), new PatrolState()},
            {typeof(AttackState), new AttackState()},
            {typeof(ReturnState), new ReturnState()},
        };

        protected virtual void Start()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            SetState<IdleState>();
        }

        private void Update()
        {
            CurrentState?.Update(this);
        }

        public void SetState<T>() where T : IState
        {
            CurrentState?.Exit(this);
            CurrentState = _states[typeof(T)];
            CurrentState.Enter(this);
        }
    }

    public interface IState
    {
        void Enter(AllyController ally);
        void Update(AllyController ally);
        void Exit(AllyController ally);
    }
}