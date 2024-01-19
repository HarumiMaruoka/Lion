using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField]
    private float _bossStatus;
    [SerializeField]
    private DropItemData[] _dropItemData;

    public float BossStatus => _bossStatus;
    public DropItemData[] DropItemData => _dropItemData;
}

[Serializable]
public struct DropItemData
{
    public int ItemID;
    public int Amount;
}