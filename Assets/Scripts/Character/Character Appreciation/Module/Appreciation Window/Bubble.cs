using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private float _duration;
    [SerializeField]
    private float _yOffset;

    private Coroutine _mainCoroutine = null;
    private Coroutine _moveCoroutine = null;
    private Coroutine _fadeCoroutine = null;

    private void Start()
    {
        Play();
    }

    public void Play()
    {
        Stop(); // 既に再生中のコルーチンは破棄する。

        _mainCoroutine = StartCoroutine(PlayAsync());
    }

    public void Stop()
    {
        if (_mainCoroutine != null) StopCoroutine(_mainCoroutine);
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
    }

    public IEnumerator PlayAsync()
    {
        var startPos = transform.position;
        var endPos = transform.position;
        endPos.y += _yOffset;
        _moveCoroutine = StartCoroutine(MoveAsync(startPos, endPos, _duration));

        _fadeCoroutine = StartCoroutine(FadeAsync(1f, 0f, _duration));

        yield return _moveCoroutine;
        yield return _fadeCoroutine;
    }

    public IEnumerator MoveAsync(Vector2 beginPos, Vector2 endPos, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.position = Vector2.Lerp(beginPos, endPos, t / duration);
            yield return null;
        }

        transform.position = endPos;
        _moveCoroutine = null;
    }

    public IEnumerator FadeAsync(float begin, float end, float duration)
    {
        var beginCol = _image.color;
        beginCol.a = begin;

        var endCol = _image.color;
        endCol.a = end;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            _image.color = Color.Lerp(beginCol, endCol, t / duration);
            yield return null;
        }

        _image.color = endCol;
        _fadeCoroutine = null;
    }
}