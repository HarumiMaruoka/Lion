using UnityEngine;
using UnityEngine.UI;

public class DamageVFX : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime;
    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private RectTransform _myRectTransform;
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private Text _text;

    private float _elapsed = 0f;

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    public void Initialize(float damageValue)
    {
        _myRectTransform.position += _offset;
        _text.text = damageValue.ToString();
    }

    public void Update()
    {
        _elapsed += Time.deltaTime * TimeScale;
        _myRectTransform.Translate(new Vector3(0f, _moveSpeed * Time.deltaTime * TimeScale));

        if (_elapsed > _lifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}