using System;
using UnityEngine;

public class CharacterIndividualInfo // 個体としてのキャラデータ
{
    public CharacterIndividualInfo(CharacterSpeciesInfo speciesInfo, int level)
    {
        _speciesInfo = speciesInfo;
        _level = level;
    }

    private CharacterSpeciesInfo _speciesInfo;
    private int _level;

    public CharacterSpeciesInfo SpeciesInfo => _speciesInfo;
    public int Level => _level;


    private WeaponBase[] _equippedWeapon = new WeaponBase[4];
    public WeaponBase[] EquippedWeapon => _equippedWeapon;
}