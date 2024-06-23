using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Glib.NovelGameEditor.Scenario.Commands.ActorActions
{
    public class Reaction : MonoBehaviour
    {
        [SerializeField] private string _reactionName;
        public string ReactionName => _reactionName;

        private Animator _animator;
        private Image _image;

        private CancellationTokenSource _cancellationTokenSource;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _image = GetComponent<Image>();
        }

        public async UniTask Play()
        {
            this.gameObject.SetActive(true);
            Show();
            try
            {
                await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1, cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            catch (OperationCanceledException)
            {
                return;
            }
            await Hide();
        }

        public void Show(string animationName = "New Animation", float duration = 0.2f)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }
            _cancellationTokenSource = new CancellationTokenSource();

            _animator.Play(animationName);
            _image.FadeAsync(1, duration, _cancellationTokenSource.Token).Forget();
        }

        public async UniTask Hide(float duration = 0.2f)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }
            _cancellationTokenSource = new CancellationTokenSource();

            await _image.FadeAsync(0, duration, _cancellationTokenSource.Token);
            this.gameObject.SetActive(false);
        }
    }
}