using System;
using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    [SerializeField]
    private WeaponBase _weaponPrefab;
    [SerializeField]
    private Sprite _weaponIcon;

    public WeaponBase WeaponPrefab => _weaponPrefab;
    public Sprite WeaponIcon => _weaponIcon;
}