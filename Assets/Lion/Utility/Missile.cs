using System;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [field: SerializeField] public Transform Target { get; set; }
    public float Period { get; set; } // 命中までの時間

    public Vector3 Velocity { get; set; }
    public float MaxAccelThreshold { get; set; } = 10f;

    protected Vector3 position;

    protected virtual void Start()
    {
        position = transform.position;
    }

    private void Update()
    {
        if (Period < 0)
        {
            Fire();
            return;
        }

        Period -= Time.deltaTime;

        var acceleration = Vector3.zero;

        Vector3 diff;
        if (Target) diff = Target.position - position;
        else diff = Vector3.zero;

        acceleration += (diff - Velocity * Period) * 2 / (Period * Period);

        if (acceleration.sqrMagnitude > MaxAccelThreshold * MaxAccelThreshold)
        {
            acceleration = acceleration.normalized * MaxAccelThreshold;
        }

        Velocity += acceleration * Time.deltaTime;
        position += Velocity * Time.deltaTime;

        transform.position = position;
    }

    protected virtual void Fire()
    {
        Destroy(gameObject);
    }
}
