using Lion.Minion.States;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Minion
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class MinionController : MonoBehaviour
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
        }

        private void Update()
        {
            _currentState.Update(this);

            // 座標バッファーの更新。
            if (CurrentPos != (Vector2)transform.position) PreviousPos = CurrentPos;
            CurrentPos = transform.position;

            // 向きの更新。
            var scale = transform.localScale;
            if (CurrentPos.x - PreviousPos.x > 0)
            {
                transform.localScale = new Vector3(InitialScale.x, InitialScale.y, InitialScale.z);
            }
            else if (CurrentPos.x - PreviousPos.x < 0)
            {
                transform.localScale = new Vector3(-InitialScale.x, InitialScale.y, InitialScale.z);
            }
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

        public Vector2 GetRandomPositionNearPlayer()
        {
            var randomX = UnityEngine.Random.Range(TopLeft.x, BottomRight.x);
            var randomY = UnityEngine.Random.Range(TopLeft.y, BottomRight.y);
            return new Vector2(randomX, randomY);
        }

        public bool IsFarFromPlayer(Vector2 position, Vector2 distance)
        {
            var xDiff = Mathf.Abs(position.x - Camera.main.transform.position.x);
            var yDiff = Mathf.Abs(position.y - Camera.main.transform.position.y);

            var xDiffIsFar = xDiff > distance.x;
            var yDiffIsFar = yDiff > distance.y;

            return xDiffIsFar || yDiffIsFar;
        }

        public bool IsFarFromPlayer(Vector2 distance)
        {
            return IsFarFromPlayer(transform.position, distance);
        }

        public bool IsFarFromPlayer()
        {
            var width = (BottomRight.x - TopLeft.x) / 2f;
            var height = (BottomRight.y - TopLeft.y) / 2f;

            return IsFarFromPlayer(new Vector2(width, height));
        }

        public bool IsTooFarFromPlayer()
        {
            var width = (BottomRight.x - TopLeft.x) / 2f + 5f;
            var height = (BottomRight.y - TopLeft.y) / 2f + 5f;

            return IsFarFromPlayer(new Vector2(width, height));
        }

        public void Fire() // アニメーションイベントから呼び出す。
        {
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            var instance = Instantiate(_bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
            instance.Controller = this;
        }
    }

    public interface IState
    {
        void Enter(MinionController minion);
        void Update(MinionController minion);
        void Exit(MinionController minion);
    }
}