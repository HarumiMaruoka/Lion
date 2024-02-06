using System;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour 
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private CharacterIndividualInfo _individualCharacterData;

    public CharacterIndividualInfo IndividualData
    {
        get => _individualCharacterData;
        set
        {
            _individualCharacterData = value;
            if (value != null)
            {
                _spriteRenderer.sprite = value.SpeciesInfo.Sprite;
            }
            else
            {
                _spriteRenderer.sprite = null;
            }
        }
    }

    private void Start()
    {
        IndividualData = null;
    }
}