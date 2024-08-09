using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Lion.UI
{
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    public class ActorMessage : MonoBehaviour
    {
        [SerializeField]
        private float _lifeTime = 1.5f;
        [SerializeField]
        private float _fadeTime = 0.3f;
        [SerializeField]
        private float _speed = 30f;
        [SerializeField]
        private RectTransform _rectTransform;

        public ActorMessagePool Pool { get; set; }

        private TMPro.TextMeshProUGUI _view;
        private string _text = "";

        public RectTransform RectTransform => _rectTransform;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                if (_view) _view.text = _text;
            }
        }

        public Color Color
        {
            get => _view.color;
            set => _view.color = value;
        }

        private void Awake()
        {
            _view = GetComponent<TMPro.TextMeshProUGUI>();
            _view.text = _text;
        }

        private async void OnEnable()
        {
            try
            {
                await UniTask.Yield(this.GetCancellationTokenOnDestroy());
                await FadeInText(this.GetCancellationTokenOnDestroy());
                await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime), cancellationToken: this.GetCancellationTokenOnDestroy());
                await FadeOutText(this.GetCancellationTokenOnDestroy());
            }
            catch (OperationCanceledException)
            {
                return;
            }

            if (Pool) Pool.Return(this);
            else Destroy(gameObject);
        }

        private void Update()
        {
            transform.position += Vector3.up * _speed * Time.deltaTime;
        }

        private async UniTask FadeInText(CancellationToken token)
        {
            var col = _view.color;
            col.a = 0;
            _view.color = col;

            for (float t = 0; t < _fadeTime; t += Time.deltaTime)
            {
                col.a = Mathf.Lerp(0, 1, t / _fadeTime);
                _view.color = col;
                await UniTask.Yield(token);
            }

            col.a = 1;
            _view.color = col;
        }

        private async UniTask FadeOutText(CancellationToken token)
        {
            var col = _view.color;
            col.a = 1;
            _view.color = col;

            for (float t = 0; t < _fadeTime; t += Time.deltaTime)
            {
                col.a = Mathf.Lerp(1, 0, t / _fadeTime);
                _view.color = col;
                await UniTask.Yield(token);
            }

            col.a = 0;
            _view.color = col;
        }
    }
}
