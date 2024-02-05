using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipCharacterSelector : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _actorImage;
    [SerializeField]
    private CharacterSelectWindow _characterSelectWindow;
    [SerializeField]
    private int _index;

    public void OnPointerClick(PointerEventData eventData)
    {
        _characterSelectWindow.OnSelected += OnSelectedCharacter;
        _characterSelectWindow.gameObject.SetActive(true);
    }

    private void OnSelectedCharacter(IndividualCharacterData selected)
    {
        _characterSelectWindow.OnSelected -= OnSelectedCharacter;
        CharacterManager.Current.CharacterBehaviours[_index].IndividualData = selected;

        if (selected != null)
        {
            _actorImage.sprite = selected.RaceData.Sprite;
        }
        else
        {
            _actorImage.sprite = null;
        }
    }
}