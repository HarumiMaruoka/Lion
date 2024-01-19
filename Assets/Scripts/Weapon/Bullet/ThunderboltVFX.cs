using System;
using UnityEngine;

public class ThunderboltVFX : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime;

    private void Start()
    {
        Destroy(this.gameObject, _lifeTime);
    }
}