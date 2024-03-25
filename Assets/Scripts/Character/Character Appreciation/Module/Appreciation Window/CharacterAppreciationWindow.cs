using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class CharacterAppreciationWindow : WindowBase
    {
        [SerializeField]
        private CharacterInventoryWindow _inventoryWindow;

        [SerializeField]
        private Image _characterImage;

        public void OpenCharacterSelectWindow()
        {
            _inventoryWindow.OnCharacterSelected += Show;
            _inventoryWindow.OnHided += OnCharacterSelectWindowHided;

            _inventoryWindow.Show();
        }

        private void OnCharacterSelectWindowHided()
        {
            _inventoryWindow.OnCharacterSelected -= Show;
            _inventoryWindow.OnHided -= OnCharacterSelectWindowHided;
        }

        [SerializeField]
        private int _dirtSpawnCount = 3;

        private void Show(CharacterIndividualData selectedCharacter)
        {
            gameObject.SetActive(true);
            _characterImage.sprite = selectedCharacter.SpeciesData.Sprite;

            // Test
            for (int i = 0; i < _dirtSpawnCount; i++)
            {
                CreateDirt(_dirtSpawnPositionLeftTop.position, _dirtSpawnPositionRightBottom.position);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            foreach (var h in _hearts) { if (h) h.Stop(); }
            foreach (var d in _activeDirties) { OnDestroyedDirtForHide(d); }
            _activeDirties.Clear();
        }

        [SerializeField]
        private Dirt _dirtPrefab;
        [SerializeField]
        private Transform _dirtPrefabParent;

        [SerializeField]
        private Transform _dirtSpawnPositionLeftTop;
        [SerializeField]
        private Transform _dirtSpawnPositionRightBottom;

        private int _dirtCount = 0;

        [SerializeField]
        private Heart[] _hearts;

        public HashSet<Dirt> _activeDirties = new HashSet<Dirt>();
        public Stack<Dirt> _inactiveDirties = new Stack<Dirt>();

        private void CreateDirt(Vector2 leftTop, Vector2 rightBottom)
        {
            if (_dirtCount == 0)
            {
                foreach (var h in _hearts) { if (h) h.Stop(); }
            }

            _dirtCount++;

            Dirt instance;
            if (_inactiveDirties.Count != 0)
            {
                instance = _inactiveDirties.Pop();
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(_dirtPrefab, _dirtPrefabParent);
            }

            var randomX = UnityEngine.Random.Range(leftTop.x, rightBottom.x);
            var randomY = UnityEngine.Random.Range(rightBottom.y, leftTop.y);
            var pos = new Vector2(randomX, randomY);

            instance.Initialize(5000f, 5000f, pos);
            _activeDirties.Add(instance);
            instance.OnDestroyed += OnDestroyedDirt;
        }

        private void OnDestroyedDirt(Dirt destroyedDirt)
        {
            destroyedDirt.gameObject.SetActive(false);
            destroyedDirt.OnDestroyed -= OnDestroyedDirt;
            _activeDirties.Remove(destroyedDirt);
            _inactiveDirties.Push(destroyedDirt);

            _dirtCount--;

            if (_dirtCount == 0)
            {
                foreach (var h in _hearts) { if (h) h.Play(); }
            }
        }

        private void OnDestroyedDirtForHide(Dirt destroyedDirt)
        {
            destroyedDirt.gameObject.SetActive(false);
            destroyedDirt.OnDestroyed -= OnDestroyedDirt;
            _inactiveDirties.Push(destroyedDirt);

            _dirtCount--;
        }
    }
}