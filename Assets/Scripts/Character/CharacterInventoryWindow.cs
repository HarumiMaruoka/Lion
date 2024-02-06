using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryWindow : MonoBehaviour
{
    [SerializeField]
    private CharacterInventoryWindowElement _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    private HashSet<CharacterInventoryWindowElement> _actives = new HashSet<CharacterInventoryWindowElement>();
    private Stack<CharacterInventoryWindowElement> _inactives = new Stack<CharacterInventoryWindowElement>();

    public event Action<CharacterIndividualInfo> OnCharacterSelected;

    private void OnEnable()
    {
        Show();
    }

    private void OnDisable()
    {
        Hide();
    }

    public void Show()
    {
        var characterCollection = CharacterInventory.Instance.Collection;
        foreach (var item in characterCollection)
        {
            CharacterInventoryWindowElement elem;
            if (_inactives.Count != 0)
            {
                elem = _inactives.Pop();
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
    }

    public void Hide()
    {
        foreach (var elem in _actives)
        {
            elem.gameObject.SetActive(false);
            _inactives.Push(elem);
            elem.OnCharacterSelected -= OnCharacterSelected;
        }
        _actives.Clear();
    }
}