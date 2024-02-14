using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryWindow : MonoBehaviour
{
    private static CharacterInventoryWindow _current = null;
    public static CharacterInventoryWindow Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _current = null;
    }

    [SerializeField]
    private CharacterInventoryWindowElement _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    private HashSet<CharacterInventoryWindowElement> _actives = new HashSet<CharacterInventoryWindowElement>();
    private Queue<CharacterInventoryWindowElement> _inactives = new Queue<CharacterInventoryWindowElement>();

    public event Action<CharacterIndividualData> OnCharacterSelected;
    public event Action OnShowed;
    public event Action OnHided;

    public void Show()
    {
        gameObject.SetActive(true);

        var characterCollection = CharacterInventory.Instance.Collection;
        foreach (var item in characterCollection)
        {
            CharacterInventoryWindowElement elem;
            if (_inactives.Count != 0)
            {
                elem = _inactives.Dequeue();
                elem.transform.SetAsLastSibling(); // Layout Groupの都合でヒエラルキーの一番下に持ってくる。
            }
            else
            {
                elem = Instantiate(_elementPrefab, _elementParent);
            }

            elem.CharacterData = item;

            elem.gameObject.SetActive(true);
            elem.OnCharacterSelected += OnCharacterSelected;
            _actives.Add(elem);
        }

        OnShowed?.Invoke();
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        foreach (var elem in _actives)
        {
            elem.gameObject.SetActive(false);
            _inactives.Enqueue(elem);
            elem.OnCharacterSelected -= OnCharacterSelected;
        }
        _actives.Clear();

        OnHided?.Invoke();
    }
}