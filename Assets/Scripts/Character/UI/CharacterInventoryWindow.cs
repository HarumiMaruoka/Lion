using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class CharacterInventoryWindow : WindowBase
    {
        private static CharacterInventoryWindow _current = null;
        public static CharacterInventoryWindow Current => _current;

        private void Awake()
        {
            if (_current)
            {
                Debug.LogError("Already exists. Have you placed two or more?");
            }
            _current = this;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _current = null;
        }

        [SerializeField]
        private CharacterInventoryWindowElement _elementPrefab;
        [SerializeField]
        private Transform _elementParent;

        private HashSet<CharacterInventoryWindowElement> _actives = new HashSet<CharacterInventoryWindowElement>();
        private Queue<CharacterInventoryWindowElement> _inactives = new Queue<CharacterInventoryWindowElement>();

        public event Action OnShowed;
        public event Action OnHided;

        private Action<CharacterIndividualData> _onCharacterSelectedBuffer; // êVÇµÇ¢óvëfÇ™ê∂ê¨Ç≥ÇÍÇΩÇ∆Ç´Ç…ìoò^Ç∑Ç◊Ç´ä÷êîåQÇàÍéûìIÇ…ãLâØÇµÇƒÇ®Ç≠óÃàÊÅB
        public event Action<CharacterIndividualData> OnCharacterSelected
        {
            add
            {
                _onCharacterSelectedBuffer += value;
                foreach (var element in _actives) element.OnCharacterSelected += value;
                foreach (var element in _inactives) element.OnCharacterSelected += value;
            }
            remove
            {
                _onCharacterSelectedBuffer -= value;
                foreach (var element in _actives) element.OnCharacterSelected -= value;
                foreach (var element in _inactives) element.OnCharacterSelected -= value;
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);

            var characterCollection = CharacterInventory.Instance.Collection;
            foreach (var item in characterCollection)
            {
                CharacterInventoryWindowElement elem;
                if (_inactives.Count != 0)
                {
                    elem = _inactives.Dequeue();
                    elem.transform.SetAsLastSibling(); // Layout GroupÇÃìsçáÇ≈ÉqÉGÉâÉãÉLÅ[ÇÃàÍî‘â∫Ç…éùÇ¡ÇƒÇ≠ÇÈÅB
                }
                else
                {
                    elem = Instantiate(_elementPrefab, _elementParent);
                    elem.OnCharacterSelected += _onCharacterSelectedBuffer;
                }

                elem.CharacterData = item;

                elem.gameObject.SetActive(true);
                _actives.Add(elem);
            }

            OnShowed?.Invoke();
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            foreach (var elem in _actives)
            {
                elem.gameObject.SetActive(false);
                _inactives.Enqueue(elem);
            }
            _actives.Clear();

            OnHided?.Invoke();
        }

        [SerializeField]
        private CharacterInformationWindow _characterInformationWindow;

        public void ShowCharacterInformationMode()
        {
            OnCharacterSelected += _characterInformationWindow.ShowCharacterInformation;
            Show();

            OnHided += OnHide;

            void OnHide()
            {
                OnHided -= OnHide;
                OnCharacterSelected -= _characterInformationWindow.ShowCharacterInformation;
            }
        }
    }
}