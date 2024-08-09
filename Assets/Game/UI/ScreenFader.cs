using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.UI
{
    [RequireComponent(typeof(Image))]
    public class ScreenFader : MonoBehaviour
    {
        public static ScreenFader Instance { get; private set; }

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            if (Instance)
            {
                Debug.LogWarning("Multiple ScreenFader instances detected. Destroying the newest one.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private Coroutine _fadeCoroutine;

        public void FadeIn(float fadeDuration = 0.5f, Action onComplete = null)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            _fadeCoroutine = StartCoroutine(FadeInAsync(fadeDuration, onComplete));
        }

        public void FadeOut(float fadeDuration = 0.5f, Action onComplete = null)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            _fadeCoroutine = StartCoroutine(FadeOutAsync(fadeDuration, onComplete));
        }

        private bool _isFading = false;
        public bool IsFading => _isFading;

        private IEnumerator FadeInAsync(float fadeDuration, Action onComplete)
        {
            _isFading = true;
            _image.enabled = true;
            yield return _image.FadeInAsync(fadeDuration);
            _fadeCoroutine = null;
            _isFading = false;

            onComplete?.Invoke();
        }

        private IEnumerator FadeOutAsync(float fadeDuration, Action onComplete)
        {
            _isFading = true;
            _image.enabled = true;
            yield return _image.FadeOutAsync(fadeDuration);
            _image.enabled = false;
            _fadeCoroutine = null;
            _isFading = false;

            onComplete?.Invoke();
        }
    }
}