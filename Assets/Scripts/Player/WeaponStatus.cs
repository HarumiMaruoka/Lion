using System;
using UnityEngine;

[Serializable]
public struct WeaponStatus
{
    [SerializeField]
    private float _attackPower; // UŒ‚—Í
    [SerializeField]
    private float _speed;      // UŒ‚‘¬“x
    [SerializeField]
    private float _duration;   // UŒ‚Œø‰ÊŽžŠÔ
    [SerializeField]
    private float _area;       // UŒ‚”ÍˆÍ
    [SerializeField]
    private float _cooldown;   // UŒ‚ŠÔŠu
    [SerializeField]
    private int _amount;       // UŒ‚‰ñ”

    public WeaponStatus(float attackPower = default, float attackSpeed = default, float attackDuration = default, float attackArea = default, float attackCooldown = default, int attackAmount = default, int revival = default, float magnet = default, float luck = default, float growth = default, float greed = default, float curse = default)
    {
        _attackPower = attackPower;
        _speed = attackSpeed;
        _duration = attackDuration;
        _area = attackArea;
        _cooldown = attackCooldown;
        _amount = attackAmount;
    }

    public static WeaponStatus Parse(string[] csvRow)
    {
        WeaponStatus result = new WeaponStatus();

        if (csvRow == null)
        {
            throw new ArgumentNullException(nameof(csvRow));
        }

        if (!float.TryParse(csvRow[0], out result._attackPower) ||
            !float.TryParse(csvRow[1], out result._speed) ||
            !float.TryParse(csvRow[2], out result._duration) ||
            !float.TryParse(csvRow[3], out result._area) ||
            !float.TryParse(csvRow[4], out result._cooldown) ||
            !int.TryParse(csvRow[5], out result._amount))
        {
            throw new FormatException("Invalid data format. Unable to parse.");
        }

        return result;
    }

    public float AttackPower => _attackPower;
    public float Speed => _speed;
    public float Duration => _duration;
    public float Area => _area;
    public float Cooldown => _cooldown;
    public int Amount => _amount;

    public static WeaponStatus operator +(WeaponStatus a, WeaponStatus b)
    {
        WeaponStatus result = new WeaponStatus();

        result._attackPower = a._attackPower + b._attackPower;
        result._speed = a._speed + b._speed;
        result._duration = a._duration + b._duration;
        result._area = a._area + b._area;
        result._cooldown = a._cooldown + b._cooldown;
        result._amount = a._amount + b._amount;

        return result;
    }
}