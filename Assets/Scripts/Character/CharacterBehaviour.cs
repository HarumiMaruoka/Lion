using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Character
{
    public class CharacterBehaviour : MonoBehaviour, ISkillUser, IPointerClickHandler
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

        public Transform Transform => transform;

        public int SkillID => _individualCharacterData.SpeciesData.SkillID;

        public ActorStatus Status => throw new NotImplementedException();

        private void Start()
        {
            IndividualData = null;
        }

        private Coroutine _skillCoroutine = null;

        public void UseSkill()
        {
            if (_individualCharacterData == null)
            {
                return;
            }

            if (_skillCoroutine != null)
            {
                StopCoroutine(_skillCoroutine);
            }

            _skillCoroutine = StartCoroutine(
                _individualCharacterData.PlaySkillAsync(this, () => _skillCoroutine = null));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UseSkill();
        }
    }
}