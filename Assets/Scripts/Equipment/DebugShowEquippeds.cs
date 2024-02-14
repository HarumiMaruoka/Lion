using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DebugShowEquippeds : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(ShowEquippeds);
    }

    private StringBuilder _stringBuilder = new StringBuilder();

    private void ShowEquippeds()
    {
        _stringBuilder.Clear();

        var equippedCharacterManager = EquipCharacterManager.Current;
        var equippedCharacterCount = equippedCharacterManager.EquippableCharacterCount;
        for (int i = 0; i < equippedCharacterCount; i++)
        {
            var equippedCharacter = equippedCharacterManager.GetEquippedCharacter(i);
            if (equippedCharacter != null)
            {
                _stringBuilder.Append($"Character Index: {i} is {equippedCharacter.SpeciesData.Name}.\n");

                var equippedWeapons = equippedCharacter.EquippedWeapons;
                for (int j = 0; j < equippedWeapons.Length; j++)
                {
                    if (equippedWeapons[j] is not null)
                    {
                        _stringBuilder.Append($"Weapon Index: {j} is {equippedWeapons[j].ToString()}.\n");
                    }
                    else
                    {
                        _stringBuilder.Append($"Weapon Index: {j} is null.\n");
                    }
                }
            }
            else
            {
                _stringBuilder.Append($"Character Index: {i} is null.\n");
            }
        }

        Debug.Log(_stringBuilder.ToString());
    }
}