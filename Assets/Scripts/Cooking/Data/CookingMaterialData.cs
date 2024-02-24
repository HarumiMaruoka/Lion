using System;
using UnityEngine;

[CreateAssetMenu]
public class CookingMaterialData : ScriptableObject
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private string _name;
    [SerializeField]
    [TextArea(2, 100)]
    private string _description;
    [SerializeField]
    private Sprite _sprite;

    public int ID => _id;
    public string Name => _name;
    public string Description => _description;
    public Sprite Sprite => _sprite;
}