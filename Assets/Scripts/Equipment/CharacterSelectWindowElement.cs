using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectWindowElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _background;
    [SerializeField]
    private Image _actorImage;
    [SerializeField]
    private Text _label;

    public Image Background => _background;
    public Image ActorImage => _actorImage;
    public Text Label => _label;

    private IndividualCharacterData _characterData;

    public IndividualCharacterData CharacterData
    {
        get { return _characterData; }
        set
        {
            _characterData = value;
            if (value != null)
            {
                _actorImage.sprite = value.RaceData.Sprite;
                _label.text = value.ToString();
            }
            else
            {
                _actorImage.sprite = null;
                _label.text = "value is null.";
            }
        }
    }

    public event Action<IndividualCharacterData> OnSelected;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelected?.Invoke(_characterData);
    }
}