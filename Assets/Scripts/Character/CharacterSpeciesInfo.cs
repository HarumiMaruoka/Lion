using System;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSpeciesInfo : ScriptableObject // 種族としてのキャラデータ
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private string _name;
    [SerializeField]
    private Sprite _sprite;

    public int ID => _id;
    public string Name => _name;
    public Sprite Sprite => _sprite;

    public override string ToString()
    {
        return $"CharacterID: {_id}, Name: {_name}";
    }
}