using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    private float TimeScale => GameSpeedManager.Instance.TimeScale;
    private Vector2 _dir;
    private WeaponStatus _status;

    public void Initialize(WeaponStatus status, Vector2 dir)
    {
        _status = status;
        _dir = dir.normalized;
        _position = transform.position;
    }

    private Vector2 _position;

    private void Update()
    {
        _position += _dir * _moveSpeed * TimeScale * _status.Speed * Time.deltaTime;
        transform.position = _position;

        if (!this.IsInGameZone()) // GameZoneÇÃäOÇ…èoÇΩÇÁîjä¸ÅB
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyController enemy))
        {
            enemy.Damage(_status.AttackPower);
            Destroy(gameObject);
            return;
        }
    }
}