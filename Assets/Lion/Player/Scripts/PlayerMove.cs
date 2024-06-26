using Lion.Gem;
using Lion.Gold;
using Lion.Manager;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMove : MonoBehaviour, IGoldCollector, IGemCollector
    {
        public PlayerManager PlayerManager => PlayerManager.Instance;

        [SerializeField]
        private Button _manualMoveButton;
        [SerializeField]
        private Button _autoMoveButton;

        [SerializeField]
        private Sprite _activeSprite;
        [SerializeField]
        private Sprite _inactiveSprite;

        [SerializeField]
        private VirtualJoystick _virtualJoystick;

        private Rigidbody2D _rigidbody2D;

        private Action OnMoveUpdate;

        public float TimeScale => GameManager.Instance.GameSpeedManager.GameSpeed;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            ChangeMoveMode(MoveMode.Manual);

            _manualMoveButton.onClick.AddListener(() => ChangeMoveMode(MoveMode.Manual));
            _autoMoveButton.onClick.AddListener(() => ChangeMoveMode(MoveMode.Auto));
        }

        private void Update()
        {
            OnMoveUpdate?.Invoke();
        }

        private void ManualMove()
        {
            Vector2 moveDir = new();
            if (_virtualJoystick.IsDragging) moveDir = _virtualJoystick.Vector;

            moveDir += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            _rigidbody2D.velocity = moveDir.normalized * PlayerManager.Status.MoveSpeed * TimeScale;
        }

        [SerializeField]
        private Transform[] _wayPoints;

        private int _wayPointIndex = 0;

        private Vector3 _startPointForAutoMove;
        private Vector3 _targetPointForAutoMove;

        private void SetMovePosition(Vector3 previous, Vector3 next)
        {
            _startPointForAutoMove = previous;
            _targetPointForAutoMove = next;
        }

        private void AutoMove()
        {
            if (_wayPoints == null || _wayPoints.Length <= 1)
            {
                Debug.LogError("WayPoints is not set.");
                return;
            }

            // 目的地に到着済みの場合Indexを更新する。
            if (MovementUtilities.IsArrived(_startPointForAutoMove, transform.position, _targetPointForAutoMove))
            {
                var previous = _wayPoints[_wayPointIndex].position;

                _wayPointIndex++;
                if (_wayPointIndex >= _wayPoints.Length) _wayPointIndex = 0;

                var next = _wayPoints[_wayPointIndex].position;

                SetMovePosition(previous, next);
            }

            var dir = (_targetPointForAutoMove - transform.position).normalized;
            _rigidbody2D.velocity = dir * PlayerManager.Status.MoveSpeed * TimeScale;
        }

        public void CollectGold(int amount)
        {

        }

        public void CollectGem(int amount)
        {
            PlayerManager.ExpLevelManager.AddExp(amount);
        }


        public void ChangeMoveMode(MoveMode mode)
        {
            if (mode == MoveMode.Manual)
            {
                OnMoveUpdate = ManualMove;
                _manualMoveButton.image.sprite = _activeSprite;
                _autoMoveButton.image.sprite = _inactiveSprite;
            }
            else if (mode == MoveMode.Auto)
            {
                OnMoveUpdate = AutoMove;
                SetMovePosition(transform.position, _wayPoints[_wayPointIndex].position);
                _manualMoveButton.image.sprite = _inactiveSprite;
                _autoMoveButton.image.sprite = _activeSprite;
            }
        }
    }

    public enum MoveMode
    {
        Manual,
        Auto
    }
}