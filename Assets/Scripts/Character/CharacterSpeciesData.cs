using System;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSpeciesData : ScriptableObject // 種族としてのキャラデータ
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private string _name;
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    [TextArea(2, 100)]
    private string _description;

    public int ID => _id;
    public string Name => _name;
    public Sprite Sprite => _sprite;
    public string Description => _description;

    public override string ToString()
    {
        return $"CharacterID: {_id}, Name: {_name}";
    }

    [SerializeField]
    private TextAsset _levelInputData;

    private ActorStatus[] _levelData = null;

    public ActorStatus[] StatusData => _levelData ??= CreateLevelData();

    private ActorStatus[] CreateLevelData()
    {
        var csv = _levelInputData.LoadCsv();
        var result = new ActorStatus[csv.Length];

        for (int i = 0; i < csv.Length; i++)
        {
            result[i] = ActorStatus.Parse(csv[i]);
        }

        return result;
    }

    public ActorStatus GetStatus(int level)
    {
        return default;

        if (_levelData == null) { _levelData = CreateLevelData(); }

        if (_levelData == null || _levelData.Length == 0)
        {
            Debug.Log($"レベルのデータがありません。");
            return default;
        }

        if (level < 0 || level >= _levelData.Length)
        {
            Debug.Log($"範囲外のレベルが選択されました。Name: {Name},Level: {level}.");
            return default;
        }

        return _levelData[level];
    }
}