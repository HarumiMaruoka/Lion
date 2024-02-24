using System;
using UnityEngine;

public class DroppedCookingMaterial : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public Action<DroppedCookingMaterial> OnDead { get; set; }

    private int _cookingMaterialID;

    public void Initialize(Vector3 position, int cookingMaterialID)
    {
        _cookingMaterialID = cookingMaterialID;
        var data = CookingMaterialDataBase.Current.GetData(cookingMaterialID);

        transform.position = position;
        _spriteRenderer.sprite = data.Sprite;

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
        CookingMaterialInventory.Instance.Add(_cookingMaterialID);
        OnDead?.Invoke(this);
    }
}