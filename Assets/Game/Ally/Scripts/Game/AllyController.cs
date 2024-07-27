using Lion.Actor;
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

        private Vector3 _lastPosition = Vector3.left;
        private Vector3 _currentPos;
        public Vector3 Direction => (_lastPosition - transform.position).normalized;

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

            ActorContainer.Instance.Register(gameObject, this);
            ActorManager.Register(this);

            Life = Status.HP;
            _currentPos = transform.position;
        }

        private void OnDestroy()
        {
            ActorContainer.Instance.Unregister(gameObject);

            ActorManager.Unregister(this);
        }

        private void Update()
        {
            CurrentState?.Update(this);
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (Vector3.SqrMagnitude(_currentPos - transform.position) > 0.01f)
            {
                _lastPosition = _currentPos;
                _currentPos = transform.position;
            }
        }

        public void SetState<T>() where T : IState
        {
            CurrentState?.Exit(this);
            CurrentState = _states[typeof(T)];
            CurrentState.Enter(this);
        }

        public void CollectGem(int amount)
        {
            AllyData.LevelManager.ExpLevelManager.AddExp(amount);
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

        public float PhysicalPower => Status.AttackPower;

        public float MagicPower => Status.AttackPower;

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

        public void ExecuteSkill()
        {
            var angle = (Mathf.Atan2(Direction.y, Direction.x) + Mathf.PI / 2f) * Mathf.Rad2Deg;
            var skill = Instantiate(AllyData.SkillPrefab, transform.position, Quaternion.Euler(0, 0, angle));
            skill.Owner = this;
        }
    }

    public interface IState
    {
        void Enter(AllyController ally);
        void Update(AllyController ally);
        void Exit(AllyController ally);
    }
}