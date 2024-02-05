using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectWindow : MonoBehaviour
{
    [SerializeField]
    private CharacterSelectWindowElement _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    private HashSet<CharacterSelectWindowElement> _actives = new HashSet<CharacterSelectWindowElement>();
    private Stack<CharacterSelectWindowElement> _inactives = new Stack<CharacterSelectWindowElement>();

    public event Action<IndividualCharacterData> OnSelected;

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
            CharacterSelectWindowElement elem;
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
            elem.OnSelected += OnSelectedCharacter;
            _actives.Add(elem);
        }
    }

    public void Hide()
    {
        foreach (var active in _actives)
        {
            active.gameObject.SetActive(false);
            active.OnSelected -= OnSelectedCharacter;
            _inactives.Push(active);
        }
        _actives.Clear();
    }

    private void OnSelectedCharacter(IndividualCharacterData selectedCharacter)
    {
        OnSelected?.Invoke(selectedCharacter);
        gameObject.SetActive(false);
    }
}