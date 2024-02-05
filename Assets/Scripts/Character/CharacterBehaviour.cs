using System;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private IndividualCharacterData _individualCharacterData;

    public IndividualCharacterData IndividualData
    {
        get => _individualCharacterData;
        set
        {
            _individualCharacterData = value;
            if (value != null)
            {
                _spriteRenderer.sprite = value.RaceData.Sprite;
            }
            else
            {
                _spriteRenderer.sprite = null;
            }
        }
    }

    private void Start()
    {
        IndividualData = null;
    }

    private WeaponBase[] _equippedWeapon = new WeaponBase[4];
    public WeaponBase[] EquippedWeapon => _equippedWeapon;

}