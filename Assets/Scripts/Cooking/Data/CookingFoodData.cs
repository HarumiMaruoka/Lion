using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CookingFoodData : ScriptableObject
{
    #region Fields and properties
    [SerializeField]
    private int _id;
    [SerializeField]
    private string _name;
    [SerializeField]
    [TextArea(2, 100)]
    private string _description;
    [SerializeField]
    private Sprite _sprite;

    [SerializeField]
    private int _cookingMaterialID1;
    [SerializeField]
    private int _cookingMaterialID2;
    [SerializeField]
    private int _cookingMaterialID3;

    [SerializeReference, SubclassSelector]
    private ICookingEffect _cookingEffect;

    public int ID => _id;
    public string Name => _name;
    public string Description => _description;
    public Sprite Sprite => _sprite;

    public int CookingMaterialID1 => _cookingMaterialID1;
    public int CookingMaterialID2 => _cookingMaterialID2;
    public int CookingMaterialID3 => _cookingMaterialID3;

    public ICookingEffect CookingEffect => _cookingEffect;
    #endregion

    #region Equal calculate
    private List<int> _remainingMaterialID = new List<int>();

    public bool Equal(int[] materialIDs)
    {
        // 配列が存在し、適切な長さであることをチェックする。
        if (materialIDs == null || materialIDs.Length != 3)
            return false;

        // 要求料理素材リストを作成し、渡されたデータが正しいかどうかチェックする。
        _remainingMaterialID.Clear();

        _remainingMaterialID.Add(_cookingMaterialID1);
        _remainingMaterialID.Add(_cookingMaterialID2);
        _remainingMaterialID.Add(_cookingMaterialID3);

        foreach (var item in materialIDs)
        {
            if (!_remainingMaterialID.Remove(item))
                return false;
        }

        return true;
    }
    #endregion
}