using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private string _name;
    private int _itemID;

    public string Name => _name;
    public int ItemID => _itemID;

    public Action<DroppedItem> OnDead { get; internal set; }

    public void Initialize(Vector3 position, ItemData data)
    {
        transform.position = position;
        _spriteRenderer.sprite = data.Sprite;
        _name = data.Name;
        _itemID = data.ID;

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

    private void OnCollectAnimationCompleted()
    {
        var itemData = ItemManager.Current.GetItemData(_itemID);

        ItemInventory.Instance.ChangeItemCount(itemData, 1);
        OnDead?.Invoke(this);
    }
}