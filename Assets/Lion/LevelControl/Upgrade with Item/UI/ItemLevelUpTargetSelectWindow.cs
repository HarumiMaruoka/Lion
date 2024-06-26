using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI.UpgradeWithItem
{
    /// <summary>
    /// アイテムを消費してレベルアップするキャラクターを選択するウィンドウ。
    /// </summary>
    public class ItemLevelUpTargetSelectWindow : MonoBehaviour
    {
        [SerializeField]
        private ActorIcon _prefab;
        [SerializeField]
        private Transform _parent;

        private Dictionary<IItemLevelUpable, ActorIcon> _elements = new();

        private void Start()
        {
            Initialize();

            LevelManager.Instance.ItemLevelUpableContainer.OnAdded += OnAdded;
            LevelManager.Instance.ItemLevelUpableContainer.OnRemoved += OnRemoved;
        }

        private void OnDestroy()
        {
            LevelManager.Instance.ItemLevelUpableContainer.OnAdded -= OnAdded;
            LevelManager.Instance.ItemLevelUpableContainer.OnRemoved -= OnRemoved;
        }

        private void Initialize()
        {
            OnSelected += OnSelectedTarget;

            foreach (var item in LevelManager.Instance.ItemLevelUpableContainer)
            {
                OnAdded(item);
            }
        }

        public event Action<IItemLevelUpable> OnSelectedBuffer;
        public event Action<IItemLevelUpable> OnSelected
        {
            add
            {
                OnSelectedBuffer += value;
                foreach (var element in _elements)
                {
                    element.Value.OnClicked += value;
                }
            }
            remove
            {
                OnSelectedBuffer -= value;
                foreach (var element in _elements)
                {
                    element.Value.OnClicked -= value;
                }
            }
        }

        private void OnAdded(IItemLevelUpable item)
        {
            var element = Instantiate(_prefab, _parent);
            element.SetItem(item);
            element.OnClicked += OnSelectedBuffer;
            _elements.Add(item, element);
        }

        private void OnRemoved(IItemLevelUpable item)
        {
            if (_elements.Remove(item, out var element))
            {
                element.OnClicked -= OnSelectedBuffer;
                Destroy(element.gameObject);
            }
        }

        [SerializeField]
        private ItemLevelUpWindow _itemLevelUpWindow;

        public void OnSelectedTarget(IItemLevelUpable target)
        {
            _itemLevelUpWindow.Open(target);
        }
    }
}