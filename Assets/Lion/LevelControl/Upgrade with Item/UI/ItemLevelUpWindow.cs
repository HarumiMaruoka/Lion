using Lion.Item;
using Lion.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI.UpgradeWithItem
{
    public class ItemLevelUpWindow : MonoBehaviour
    {
        public IItemLevelUpable Target;
        public int Level => Target.Level;
        public int NextLevel { get; private set; }
        public int MaxLevel => Target.MaxLevel;

        /// <summary>
        /// Key: �A�C�e��ID, Value: �K�v��
        /// </summary>
        private Dictionary<int, int> _levelUpCosts = new();

        [SerializeField]
        private Image _actorSpriteView;

        [SerializeField]
        private Button _nextLevelUpButton;
        [SerializeField]
        private Button _nextLevelDownButton;
        [SerializeField]
        private Button _applyLevelButton;

        private void Start()
        {
            _nextLevelUpButton.onClick.AddListener(NextLevelUp);
            _nextLevelDownButton.onClick.AddListener(NextLevelDown);
            _applyLevelButton.onClick.AddListener(ApplyLevel);
        }

        public void Open(IItemLevelUpable target)
        {
            Target = target;
            NextLevel = Level;

            _actorSpriteView.sprite = Target.ActorSprite;
            UpdateView();

            _levelUpCosts.Clear();
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void ApplyLevel()
        {
            // �A�C�e��������鏈��
            Debug.Log("�����ɃA�C�e��������鏈���������B");

            if (Level < MaxLevel)
            {
                Target.LevelUp(NextLevel);
                NextLevel = Level;
            }

            Open(Target);
        }

        public void NextLevelUp()
        {
            if (NextLevel < MaxLevel)
            {
                NextLevel++;
                // ����A�C�e���̌v�Z
                var costs = Target.GetLevelUpCosts(NextLevel);
                foreach (var c in costs)
                {
                    if (_levelUpCosts.ContainsKey(c.ItemID))
                    {
                        _levelUpCosts[c.ItemID] += c.Amount;
                    }
                    else
                    {
                        _levelUpCosts.Add(c.ItemID, c.Amount);
                    }
                }
                UpdateView();
            }
        }

        public void NextLevelDown()
        {
            if (NextLevel > Level)
            {
                // ����A�C�e���̌v�Z
                var costs = Target.GetLevelUpCosts(NextLevel);
                foreach (var c in costs)
                {
                    if (_levelUpCosts.ContainsKey(c.ItemID))
                    {
                        _levelUpCosts[c.ItemID] -= c.Amount;
                    }
                    else
                    {
                        _levelUpCosts.Add(c.ItemID, -c.Amount);
                    }
                }

                NextLevel--;
                UpdateView();
            }
        }

        public void LevelUpMax()
        {
            NextLevel = MaxLevel;
            // ����A�C�e���̌v�Z
            _levelUpCosts.Clear();
            for (var i = Level; i < MaxLevel; i++)
            {
                var costs = Target.GetLevelUpCosts(i + 1);
                foreach (var c in costs)
                {
                    if (_levelUpCosts.ContainsKey(c.ItemID))
                    {
                        _levelUpCosts[c.ItemID] += c.Amount;
                    }
                    else
                    {
                        _levelUpCosts.Add(c.ItemID, c.Amount);
                    }
                }
            }

            UpdateView();
        }

        [SerializeField]
        private TMPro.TextMeshProUGUI _levelLabel;
        [SerializeField]
        private TMPro.TextMeshProUGUI _nextLevelLabel;
        [SerializeField]
        private TMPro.TextMeshProUGUI _statusLabel;
        [SerializeField]
        private TMPro.TextMeshProUGUI _nextStatusLabel;
        [SerializeField]
        private ItemIcon _itemIconPrefab;
        [SerializeField]
        private Transform _itemIconParent;

        private HashSet<ItemIcon> _activeItemIcon = new HashSet<ItemIcon>();
        private Queue<ItemIcon> _inactiveItemIcon = new Queue<ItemIcon>();

        public void UpdateView()
        {
            // ���x���\���̍X�V
            _levelLabel.text = $"Lv: {Level}";
            _nextLevelLabel.text = $"Lv: {NextLevel}";
            _statusLabel.text = Target.GetStatus().ToString();
            _nextStatusLabel.text = Target.GetStatus(NextLevel).ToString();
            // ����A�C�e���̕\��
            foreach (var icon in _activeItemIcon)
            {
                icon.gameObject.SetActive(false);
                _inactiveItemIcon.Enqueue(icon);
            }
            _activeItemIcon.Clear();
            foreach (var c in _levelUpCosts)
            {
                var itemData = ItemManager.Instance.ItemSheet.GetItemData(c.Key);
                var needCount = c.Value;
                var icon = GetItemIcon();
                icon.ApplyView(itemData, needCount);
            }
        }

        public ItemIcon GetItemIcon()
        {
            if (_inactiveItemIcon.Count > 0)
            {
                var icon = _inactiveItemIcon.Dequeue();
                icon.gameObject.SetActive(true);
                _activeItemIcon.Add(icon);
                return icon;
            }
            else
            {
                var icon = Instantiate(_itemIconPrefab, _itemIconParent);
                _activeItemIcon.Add(icon);
                return icon;
            }
        }
    }
}