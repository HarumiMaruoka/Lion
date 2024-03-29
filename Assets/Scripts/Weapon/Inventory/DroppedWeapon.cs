using System;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public Action<DroppedWeapon> OnDead { get; set; }

    private WeaponData _weaponData;

    public void Initialize(Vector3 position, WeaponData data)
    {
        _weaponData = data;

        transform.position = position;
        _spriteRenderer.sprite = data.WeaponIcon;

        PlayCollectAnimation();
    }

    [SerializeField]
    private float _collectAnimationInitialVelocity;
    [SerializeField]
    private float _collectAnimationDuration;

    private void PlayCollectAnimation()
    {
        var player = PlayerController.Current;
        if (player == null) return;

        var dir = (this.transform.position - player.transform.position).normalized;
        var initialVelocity = dir * _collectAnimationInitialVelocity;
        this.PlayThrowAnimation(player.transform, _collectAnimationDuration, initialVelocity,
            OnCollectAnimationCompleted);
    }

    private static bool _collectionWarningLogFlag = true; // 警告を何度も出さない為のフラグ

    private void OnCollectAnimationCompleted()
    {
        var player = PlayerController.Current;
        var weaponInventory = WeaponInventory.Current;
        var capacity = WeaponInventory.Current.InventoryCapacity;

        if (player == null)
        {
            Debug.Log("Player is Missing.");
            OnDead?.Invoke(this);
            return;
        }
        if (weaponInventory == null)
        {
            Debug.Log("Weapon Inventory is Missing.");
            OnDead?.Invoke(this);
            return;
        }
        if (WeaponInventory.Current.WeaponCollection.Count >= capacity)
        {
            if (_collectionWarningLogFlag) // 警告を何度も出さない為の処理
            {
                _collectionWarningLogFlag = false;
                Debug.Log("The weapon inventory capacity is full.");
            }

            OnDead?.Invoke(this);
            return;
        }
        _collectionWarningLogFlag = true;

        var weaponCollection = WeaponInventory.Current.WeaponCollection;
        var instance = GameObject.Instantiate(_weaponData.WeaponPrefab, player.WeaponParent);
        weaponInventory.AddWeapon(instance);
        OnDead?.Invoke(this);
    }
}