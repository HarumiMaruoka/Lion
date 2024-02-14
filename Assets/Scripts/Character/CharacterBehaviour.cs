using System;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour 
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private CharacterIndividualData _individualCharacterData;

    public CharacterIndividualData IndividualData
    {
        get => _individualCharacterData;
        set
        {
            _individualCharacterData = value;
            if (value != null)
            {
                _spriteRenderer.sprite = value.SpeciesData.Sprite;
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