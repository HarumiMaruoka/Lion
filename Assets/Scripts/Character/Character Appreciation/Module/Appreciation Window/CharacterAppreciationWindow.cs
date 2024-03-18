using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAppreciationWindow : MonoBehaviour
{
    [SerializeField]
    private Image _characterImage;

    public void Show(CharacterIndividualData selectedCharacter)
    {
        gameObject.SetActive(true);
        _characterImage.sprite = selectedCharacter.SpeciesData.Sprite;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}