using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterInventoryWindowElement : MonoBehaviour , IPointerClickHandler
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

    private CharacterIndividualInfo _characterData;

    public CharacterIndividualInfo CharacterData
    {
        get { return _characterData; }
        set
        {
            _characterData = value;
            if (value != null)
            {
                _actorImage.sprite = value.SpeciesInfo.Sprite;
                _label.text = value.ToString();
            }
            else
            {
                _actorImage.sprite = null;
                _label.text = "value is null.";
            }
        }
    }

    public event Action<CharacterIndividualInfo> OnCharacterSelected;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCharacterSelected?.Invoke(_characterData);
    }
}