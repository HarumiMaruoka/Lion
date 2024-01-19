using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private static VFXManager _current = null;
    public static VFXManager Current => _current;

    private void Awake()
    {
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
}