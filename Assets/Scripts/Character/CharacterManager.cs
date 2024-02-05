using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _current = null;
    public static CharacterManager Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
        Initialize();
    }

    private void OnDestroy()
    {
        _current = null;
    }

    [SerializeField]
    private CharacterBehaviour[] _characterBehaviours;

    public CharacterBehaviour[] CharacterBehaviours => _characterBehaviours;

    [SerializeField]
    private RaceCharacterData[] _raceCharacterData;

    private Dictionary<int, RaceCharacterData> _idToData = new Dictionary<int, RaceCharacterData>();
    private Dictionary<string, RaceCharacterData> _nameToData = new Dictionary<string, RaceCharacterData>();

    private void Initialize()
    {
        foreach (var item in _raceCharacterData)
        {
            if (!_idToData.TryAdd(item.ID, item))
            {
                Debug.LogError($"{item.ToString()}: CharacterIDが重複しています。");
            }
            if (!_nameToData.TryAdd(item.Name, item))
            {
                Debug.LogError($"{item.ToString()}: Character Nameが重複しています。");
            }
        }
    }

    public RaceCharacterData GetRaceCharacterData(int id)
    {
        if (_idToData.TryGetValue(id, out RaceCharacterData result))
        {
            return result;
        }
        Debug.Log($"ID {id} is Missing.");
        return null;
    }
    public RaceCharacterData GetRaceCharacterData(string name)
    {
        if (_nameToData.TryGetValue(name, out RaceCharacterData result))
        {
            return result;
        }
        Debug.Log($"{name} is Missing.");
        return null;
    }

    [SerializeField]
    private DroppedCharacter _droppedCharacterPrefab;

    private HashSet<DroppedCharacter> _activeItems = new HashSet<DroppedCharacter>();
    private Stack<DroppedCharacter> _inactiveItems = new Stack<DroppedCharacter>();

    public void DropCharacter(Vector3 position, int characterID, float probability) // probabilityは確率を表現する。0.0から1.0で判断する。0の方が出にくく、1の方が出やすい。
    {
        var random = UnityEngine.Random.Range(0f, 1f);

        if (probability > random) return; // 確率を下回ればアイテムを生成しない。

        // 指定されたCharacterIDのキャラが見つからなければ警告を出してリターン。
        var characterData = GetRaceCharacterData(characterID);
        if (characterData == null) return;

        // Create Character.
        DroppedCharacter item = null;
        if (_inactiveItems.Count == 0)
            item = Instantiate(_droppedCharacterPrefab, this.transform);
        else
            item = _inactiveItems.Pop();

        // Activate Character
        _activeItems.Add(item);
        item.gameObject.SetActive(true);
        item.Initialize(position, characterData);
        item.OnDead += DeleteItem;
    }

    private void DeleteItem(DroppedCharacter item)
    {
        item.OnDead -= DeleteItem;
        item.gameObject.SetActive(false);
        _activeItems.Remove(item);
        _inactiveItems.Push(item);
    }
}