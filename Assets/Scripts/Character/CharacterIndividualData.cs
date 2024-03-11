using System;
using System.Linq;
using UnityEngine;

public class CharacterIndividualData : IActor// 個体としてのキャラデータ
{
    public CharacterIndividualData(CharacterSpeciesData speciesData, int level)
    {
        _speciesData = speciesData;
        _level = level;
    }

    private int _equipIndex = -1;
    private CharacterSpeciesData _speciesData;
    private int _level;

    public string Name => _speciesData.Name;
    public int EquipIndex => _equipIndex;
    public CharacterSpeciesData SpeciesData => _speciesData;
    public int Level => _level;

    private WeaponBase[] _equippedWeapon = new WeaponBase[4];
    public WeaponBase[] EquippedWeapons => _equippedWeapon;

    public ActorStatus Status => _speciesData.GetStatus(_level);

    public ISkill Skill => null; // TODO: Characterのスキルの実装。

    private IProfile[] _dummy = { new DummyProfile(), new DummyProfile(), new DummyProfile(), new DummyProfile() };
    public IProfile[] Profile => _dummy; // TODO: Characterのプロフィールの実装。

    public CharacterBehaviour CharacterBehaviour
    {
        get
        {
            var equippableCount = EquipCharacterManager.Current.EquippableCharacterCount;

            if (_equipIndex < 0 && _equipIndex >= equippableCount) // 範囲外。
            {
                return null;
            }

            return EquipCharacterManager.Current.GetCharacterBehaviour(_equipIndex);
        }
    }

    public float BattlePower { get => Status.BattlePower; }

    public void Equip(int index)
    {
        _equipIndex = index;

        // 武器をアクティブ化する。
        foreach (var weapon in _equippedWeapon)
        {
            var characterBehaviour = CharacterBehaviour;
            if (characterBehaviour && weapon) weapon.Activate(characterBehaviour.transform);
        }
    }

    public void Unequip()
    {
        _equipIndex = -1;

        // 武器を非アクティブ化する。
        foreach (var weapon in _equippedWeapon)
        {
            if (weapon) weapon.Inactivate();
        }
    }

    public override string ToString()
    {
        return $"Name: {_speciesData.Name}\nLevel: {_level}";
    }
}