using System;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private int _id;
    [SerializeField]
    private Sprite _sprite;

    public string Name => _name;
    public int ID => _id;
    public Sprite Sprite => _sprite;
}