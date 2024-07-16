using System;
using UnityEngine;

public class ContinualHealing : MonoBehaviour
{
    private static GameObject _healEffectPrefab;
    private static GameObject HealEffectPrefab => _healEffectPrefab ??= Resources.Load<GameObject>("HealEffect");

    public IActor Actor { get; set; }

    public float HealAmount { get; set; }
    public float HealInterval { get; set; }
    public float Duration { get; set; }

    private float _intervalTimer;
    private float _durationTimer;

    private GameObject _healEffect;

    private void Start()
    {
        _intervalTimer = HealInterval;
        _durationTimer = Duration;

        _healEffect = Instantiate(HealEffectPrefab, transform.position, Quaternion.identity, transform);
    }

    private void OnDestroy()
    {
        Destroy(_healEffect);
    }

    private void Update()
    {
        _intervalTimer -= Time.deltaTime;
        _durationTimer -= Time.deltaTime;

        if (_intervalTimer <= 0)
        {
            Actor.Heal(HealAmount);
            _intervalTimer = HealInterval;
        }

        if (_durationTimer <= 0)
        {
            Destroy(this);
        }
    }
}
