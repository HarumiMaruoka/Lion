using System;
using UnityEngine;

namespace Lion.Actor
{
    public class ActivityArea : MonoBehaviour
    {
        public static ActivityArea Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogError("ActivityArea is already exists.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        [SerializeField]
        private RectTransform _rectTransform;

        private Vector2? _cachedCanvasLeftTop;
        private Vector2? _cachedCanvasRightBottom;
        private Vector3? _cachedWorldLeftTop;
        private Vector3? _cachedWorldRightBottom;
        private int _lastFrameCalculated = -1;

        private void Update()
        {
            // フレームが変わったらキャッシュをクリア
            if (Time.frameCount != _lastFrameCalculated)
            {
                _cachedCanvasLeftTop = null;
                _cachedCanvasRightBottom = null;
                _cachedWorldLeftTop = null;
                _cachedWorldRightBottom = null;
                _lastFrameCalculated = Time.frameCount;
            }
        }

        public Vector2 CanvasLeftTop
        {
            get
            {
                if (!_cachedCanvasLeftTop.HasValue)
                {
                    _cachedCanvasLeftTop = new Vector2(_rectTransform.position.x - _rectTransform.rect.width / 2, _rectTransform.position.y + _rectTransform.rect.height / 2);
                }
                return _cachedCanvasLeftTop.Value;
            }
        }

        public Vector2 CanvasRightBottom
        {
            get
            {
                if (!_cachedCanvasRightBottom.HasValue)
                {
                    _cachedCanvasRightBottom = new Vector2(_rectTransform.position.x + _rectTransform.rect.width / 2, _rectTransform.position.y - _rectTransform.rect.height / 2);
                }
                return _cachedCanvasRightBottom.Value;
            }
        }

        public Vector3 WorldLeftTop
        {
            get
            {
                if (!_cachedWorldLeftTop.HasValue)
                {
                    _cachedWorldLeftTop = Camera.main.ScreenToWorldPoint(CanvasLeftTop);
                    _cachedWorldLeftTop = new Vector3(_cachedWorldLeftTop.Value.x, _cachedWorldLeftTop.Value.y, 0);
                }
                return _cachedWorldLeftTop.Value;
            }
        }

        public Vector3 WorldRightBottom
        {
            get
            {
                if (!_cachedWorldRightBottom.HasValue)
                {
                    _cachedWorldRightBottom = Camera.main.ScreenToWorldPoint(CanvasRightBottom);
                    _cachedWorldRightBottom = new Vector3(_cachedWorldRightBottom.Value.x, _cachedWorldRightBottom.Value.y, 0);
                }
                return _cachedWorldRightBottom.Value;
            }
        }

        public bool IsInArea(Vector3 position)
        {
            return WorldLeftTop.x <= position.x && position.x <= WorldRightBottom.x && WorldRightBottom.y <= position.y && position.y <= WorldLeftTop.y;
        }

        public Vector3 GetRandomPosition()
        {
            return new Vector3(UnityEngine.Random.Range(WorldLeftTop.x, WorldRightBottom.x), UnityEngine.Random.Range(WorldRightBottom.y, WorldLeftTop.y), 0);
        }

        public bool IsFarFromArea(Vector3 position, float margin = 0f)
        {
            var leftTop = new Vector3(WorldLeftTop.x - margin, WorldLeftTop.y + margin, 0);
            var rightBottom = new Vector3(WorldRightBottom.x + margin, WorldRightBottom.y - margin, 0);
            return leftTop.x >= position.x || position.x >= rightBottom.x || rightBottom.y >= position.y || position.y >= leftTop.y;
        }
    }
}