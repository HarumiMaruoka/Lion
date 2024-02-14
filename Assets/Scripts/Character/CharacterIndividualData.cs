using System;
using UnityEngine;

public class CharacterIndividualData // 個体としてのキャラデータ
{
    public CharacterIndividualData(CharacterSpeciesData speciesData, int level)
    {
        _speciesData = speciesData;
        _level = level;
    }

    private CharacterSpeciesData _speciesData;
    private int _level;

    public CharacterSpeciesData SpeciesData => _speciesData;
    public int Level => _level;


    private WeaponBase[] _equippedWeapon = new WeaponBase[4];
    public WeaponBase[] EquippedWeapons => _equippedWeapon;

    public override string ToString()
    {
        return $"Name: {_speciesData.Name}\nLevel: {_level}";
    }
}