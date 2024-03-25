using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class CharacterInformationWindow : WindowBase
    {
        #region Common Elements
        public void ShowCharacterInformation(CharacterIndividualData character)
        {
            gameObject.gameObject.SetActive(true);

            if (character == null)
            {
                Debug.Log("Character is null.");
                return;
            }

            ApplyNameView(character);
            ApplyCharacterImage(character);
            ApplyStatusView(character);
            ApplySkillView(character);
            ApplyCharacterProfile(character);
        }
        #endregion

        #region Name View
        [Header("Name")]
        [SerializeField]
        private Text _name;

        private void ApplyNameView(CharacterIndividualData character)
        {
            _name.text = "Name: " + character.SpeciesData.Name;
        }
        #endregion

        #region Character Image
        [Header("Character Image")]
        [SerializeField]
        private Image _characterImage;

        private void ApplyCharacterImage(CharacterIndividualData character)
        {
            _characterImage.sprite = character.SpeciesData.Sprite;
        }
        #endregion

        #region Status View
        [Header("Status")]
        [SerializeField]
        private Text _maxLife;
        [SerializeField]
        private Text _moveSpeed;
        [SerializeField]
        private Text _attackPower;
        [SerializeField]
        private Text _defense;
        [SerializeField]
        private Text _dexterity;
        [SerializeField]
        private Text _luck;

        private void ApplyStatusView(CharacterIndividualData character)
        {
            _maxLife.text = "Max Life: " + character.Status.MaxLife.ToString();
            _moveSpeed.text = "Move Speed: " + character.Status.MoveSpeed.ToString();
            _attackPower.text = "Attack Power: " + character.Status.AttackPower.ToString();
            _defense.text = "Defense: " + character.Status.Defense.ToString();
            _dexterity.text = "Dexterity: " + character.Status.Dexterity.ToString();
            _luck.text = "Luck: " + character.Status.Luck.ToString();
        }
        #endregion

        #region Skill View
        [Header("Skill")]
        [SerializeField]
        private Text _skillDescription;

        private void ApplySkillView(CharacterIndividualData character)
        {
            if (character.Skill != null)
            {
                _skillDescription.text = character.Skill.Description;
            }
            else
            {
                _skillDescription.text = "Character Skill is null.";
            }
        }
        #endregion

        #region Profile View
        [Header("Profile")]
        [SerializeField]
        private ProfileView _profileViewPrefab;
        [SerializeField]
        private Transform _profileViewParent;

        private HashSet<ProfileView> _actives = new HashSet<ProfileView>();
        private Stack<ProfileView> _inactives = new Stack<ProfileView>();

        private ProfileView GetProfileView()
        {
            ProfileView result;
            if (_inactives.Count != 0)
            {
                result = _inactives.Pop();
            }
            else
            {
                result = Instantiate(_profileViewPrefab, _profileViewParent);
            }

            result.gameObject.SetActive(true);
            _actives.Add(result);
            return result;
        }

        private void ApplyCharacterProfile(CharacterIndividualData character)
        {
            // Dispose old elements
            foreach (var oldElem in _actives)
            {
                oldElem.gameObject.SetActive(false);
                _inactives.Push(oldElem);
            }
            _actives.Clear();

            // Show character profiles
            if (character.Profile == null)
            {
                Debug.Log("Character Profile is null.");
                return;
            }
            foreach (var elem in character.Profile)
            {
                var profileView = GetProfileView();

                if (elem.IsValid)
                {
                    profileView.Caption.text = elem.Caption;
                    profileView.Content.text = elem.ContentedText;
                }
                else
                {
                    profileView.Invalid(); // ロック状態のプロフィール。主人公と仲良くなる事で解放される。
                }
            }
        }
        #endregion
    }
}