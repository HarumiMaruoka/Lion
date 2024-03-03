using System;
using UnityEngine;

[Serializable]
public struct ActorStatus
{
    [SerializeField]
    private float _maxLife;     // 最大ライフ
    [SerializeField]
    private float _moveSpeed;   // 移動スピード
    [SerializeField]
    private float _attackPower; // 攻撃力
    [SerializeField]
    private float _defense;     // 防御力
    [SerializeField]
    private float _dexterity;   // 器用
    [SerializeField]
    private float _luck;        // 運

    public float MaxLife => _maxLife;
    public float MoveSpeed => _moveSpeed;
    public float AttackPower => _attackPower;
    public float Defense => _defense;
    public float Dexterity => _dexterity;
    public float Luck => _luck;

    public float Sum // 合計戦闘力。ボスとの戦闘の際、勝ち負けの判定で使う。
    {
        get
        {
            float sum = 0;

            sum += _maxLife;
            sum += _moveSpeed;
            sum += _attackPower;
            sum += _defense;
            sum += _dexterity;
            sum += _luck;

            return sum;
        }
    }

    public static ActorStatus operator +(ActorStatus a, ActorStatus b)
    {
        ActorStatus result = new ActorStatus();

        result._maxLife = a._maxLife + b._maxLife;
        result._moveSpeed = a._moveSpeed + b._moveSpeed;
        result._attackPower = a._attackPower + b._attackPower;
        result._defense = a._defense + b._defense;
        result._dexterity = a._dexterity + b._dexterity;
        result._luck = a._luck + b._luck;

        return result;
    }

    public static ActorStatus Parse(string[] csvRow)
    {
        if (csvRow == null)
        {
            throw new ArgumentNullException(nameof(csvRow));
        }

        ActorStatus result = new ActorStatus();

        if (!float.TryParse(csvRow[0], out result._maxLife) ||
            !float.TryParse(csvRow[1], out result._moveSpeed) ||
            !float.TryParse(csvRow[2], out result._attackPower) ||
            !float.TryParse(csvRow[3], out result._defense) ||
            !float.TryParse(csvRow[4], out result._dexterity) ||
            !float.TryParse(csvRow[5], out result._luck))
        {
            throw new FormatException("Invalid data format. Unable to parse.");
        }

        result._moveSpeed *= 0.1f; // 移動速度は0.1倍する。（早すぎるので）

        return result;
    }
}