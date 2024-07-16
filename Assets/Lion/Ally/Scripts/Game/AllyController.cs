using Lion.Gem;
using Lion.Gold;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Ally
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class AllyController : MonoBehaviour, IActor
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

            GemCollectorContainer.Instance.Register(gameObject, this);
            GoldCollectorContainer.Instance.Register(gameObject, this);

            Life = Status.HP;
        }

        private void OnDestroy()
        {
            GemCollectorContainer.Instance.Unregister(gameObject);
            GoldCollectorContainer.Instance.Unregister(gameObject);
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

        public void CollectGem(int amount)
        {
            AllyData.ExpLevelManager.AddExp(amount);
        }

        public void CollectGold(int amount)
        {

        }

        private float _life;
        public event Action<float> OnLifeChanged;
        public float Life
        {
            get => _life;
            set
            {
                _life = Mathf.Clamp(value, 0f, Status.HP);
                OnLifeChanged?.Invoke(_life);
            }
        }

        public void Heal(float amount)
        {
            Life += amount;
        }

        public void Revive()
        {
            Life = Status.HP;
        }

        public void Damage(float amount)
        {
            Life -= amount;
        }
    }

    public interface IState
    {
        void Enter(AllyController ally);
        void Update(AllyController ally);
        void Exit(AllyController ally);
    }
}