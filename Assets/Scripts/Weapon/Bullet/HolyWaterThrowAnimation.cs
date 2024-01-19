using System;
using System.Collections;
using UnityEngine;

public class HolyWaterThrowAnimation : MonoBehaviour
{
    [SerializeField]
    private HolyWater _holyWaterPrefab;

    [SerializeField]
    private float _duration;

    private Vector2 _targetPosition;

    private WeaponStatus _status;

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    public void Initialize(WeaponStatus status, Vector2 targetPosition)
    {
        _status = status;
        _targetPosition = targetPosition;
    }

    private void Start()
    {
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        var startPos = transform.position;
        var endPos = _targetPosition;
        yield return Animation(startPos, endPos);
        var instance = Instantiate(_holyWaterPrefab, transform.position, Quaternion.identity, transform.parent);
        instance.Initialize(_status);
        Destroy(gameObject);
    }

    private IEnumerator Animation(Vector2 startPosition, Vector2 endPosition)
    {
        transform.position = startPosition;
        for (float t = 0f; t < _duration; t += Time.deltaTime * TimeScale)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, t / _duration);
            yield return null;
        }
        transform.position = endPosition;
    }
}