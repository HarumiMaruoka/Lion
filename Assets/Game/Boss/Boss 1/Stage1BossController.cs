using Lion.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Enemy.Boss
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(Stage1BossParameters))]
    public class Stage1BossController : MonoBehaviour
    {
        private void Start()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Parameters = GetComponent<Stage1BossParameters>();

            _maxLife = Parameters.MaxLife;
            CurrentLife = _maxLife;

            SetState<IdleState>();
        }

        private void Update()
        {
            _currentState?.Update(this);
            UpdateDirection();
        }

        public Rigidbody2D Rigidbody2D { get; private set; }
        public Animator Animator { get; private set; }
        public Stage1BossParameters Parameters { get; private set; }

        private IState _currentState;
        private Dictionary<Type, IState> _states =
            new Dictionary<Type, IState>() {
                { typeof(IdleState), new IdleState() },
                { typeof(ApproachingState), new ApproachingState() },
                { typeof(RetreatState), new RetreatState() },
                { typeof(ObservingState), new ObservingState() },

                { typeof(MeleeAttackState),new MeleeAttackState() },
                { typeof(RangeAttackState), new RangeAttackState() },
            };

        public void SetState<T>() where T : IState
        {
            if (_currentState != null)
            {
                _currentState.Exit(this);
            }

            _currentState = _states[typeof(T)];
            _currentState.Enter(this);
        }

        public interface IState
        {
            void Enter(Stage1BossController boss);
            void Update(Stage1BossController boss);
            void Exit(Stage1BossController boss);
        }

        private float _maxLife;
        private float _currentLife;

        public event Action<float> OnLifeChanged;

        public float CurrentLife
        {
            get => _currentLife;
            set
            {
                _currentLife = value;
                OnLifeChanged?.Invoke(_currentLife);
                if (_currentLife <= 0)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            Debug.Log("Die");
        }

        private void UpdateDirection()
        {
            // プレイヤーの方向を向く
            Vector3 direction = PlayerController.Instance.transform.position - transform.position;
            var localScale = transform.localScale;

            if (direction.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, 1);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, 1);
            }
        }


        [SerializeField]
        private MeleeAttackObject _meleeAttackObject;

        public void BeginMeleeAttack()
        {
            _meleeAttackObject.gameObject.SetActive(true);
        }

        public void EndMeleeAttack()
        {
            _meleeAttackObject.gameObject.SetActive(false);
        }

        [SerializeField]
        private RangeAttackObject _rangeAttackObject;

        public void RangeAttack()
        {
            Instantiate(_rangeAttackObject, transform.position, Quaternion.identity);
        }
    }
}