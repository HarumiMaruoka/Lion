using System;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private static VFXManager _current = null;
    public static VFXManager Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
    }

    private void OnDestroy()
    {
        _current = null;
    }

    [SerializeField]
    private DamageVFX _damageVFXPrefab;
    [SerializeField]
    private Transform _damageVFXParent;

    public void RequestDamageVFX(float damageValue, Vector2 position)
    {
        var randomOffsetX = UnityEngine.Random.Range(-20f, 20f);
        var randomOffsetY = UnityEngine.Random.Range(-20f, 20f);

        position += new Vector2(randomOffsetX, randomOffsetY);

        var damageVFX = Instantiate(_damageVFXPrefab, position, Quaternion.identity, _damageVFXParent);
        damageVFX.Initialize(damageValue);
    }

    [SerializeField]
    private GameObject _bubblePrefab;
    [SerializeField]
    private Transform _bubbleParent;

    internal void CreateBubbleVFX(Vector3 mousePosition)
    {
        if (_bubblePrefab)
            Instantiate(_bubblePrefab, mousePosition, Quaternion.identity, _bubbleParent);
        else
            Debug.Log("_bubblePrefab is unassigned.");
    }
}