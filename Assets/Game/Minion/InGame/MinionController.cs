using Lion.Actor;
using Lion.Gem;
using Lion.Gold;
using Lion.Minion.States;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Minion
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class MinionController : MonoBehaviour, IActor
    {
        [SerializeField] private MinionBullet _bulletPrefab;

        public MinionData MinionData { get; set; }
        public MinionStatus Status => MinionData.Status;

        public Rigidbody2D Rigidbody2D { get; private set; }
        public Animator Animator { get; private set; }

        public Vector2 CurrentPos { get; private set; }
        public Vector2 PreviousPos { get; private set; }

        public Vector2 Direction => CurrentPos - PreviousPos;

        public Vector2 TopLeft => Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        public Vector2 BottomRight => Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        public Vector3 InitialScale { get; private set; }

        private void Start()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();

            CurrentPos = transform.position;
            PreviousPos = transform.position;

            InitialScale = transform.localScale;

            SetState<PatrolState>();

            ActorContainer.Instance.Register(gameObject, this);

            Life = Status.HP;

            ActorManager.Register(this);
        }

        private void OnDestroy()
        {
            ActorContainer.Instance.Unregister(gameObject);

            ActorManager.Unregister(this);
        }

        private void Update()
        {
            _currentState.Update(this);

            // 座標バッファーの更新。
            if (CurrentPos != (Vector2)transform.position) PreviousPos = CurrentPos;
            CurrentPos = transform.position;
        }

        private IState _currentState;

        private Dictionary<Type, IState> _states = new Dictionary<Type, IState>()
        {
            {typeof(IdleState), new IdleState()},
            {typeof(PatrolState), new PatrolState()},
            {typeof(AttackState), new AttackState()},
            {typeof(ReturnState), new ReturnState()},
        };

        public void SetState<T>() where T : IState
        {
            _currentState?.Exit(this);
            _currentState = _states[typeof(T)];
            _currentState.Enter(this);
        }

        public void Fire() // アニメーションイベントから呼び出す。
        {
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            var instance = Instantiate(_bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
            instance.Controller = this;
        }

        public void CollectGem(int amount)
        {
            MinionData.LevelManager.ExpLevelManager.AddExp(amount);
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

        public float PhysicalPower => Status.Attack;

        public float MagicPower => Status.Attack;

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
        void Enter(MinionController minion);
        void Update(MinionController minion);
        void Exit(MinionController minion);
    }
}