using System;
using UnityEngine;

public class IndividualCharacterData // 個体としてのキャラデータ
{
    public IndividualCharacterData(RaceCharacterData raceData, int level)
    {
        _raceData = raceData;
        _level = level;
    }

    private RaceCharacterData _raceData;
    private int _level;

    public RaceCharacterData RaceData => _raceData;
    public int Level => _level;
}