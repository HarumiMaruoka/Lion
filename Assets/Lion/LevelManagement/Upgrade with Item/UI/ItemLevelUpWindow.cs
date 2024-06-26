using Lion.Item;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI.UpgradeWithItem
{
    public class ItemLevelUpWindow : MonoBehaviour
    {
        public IItemLevelable Target;
        public int Level => Target.ItemLevelManager.CurrentLevel;
        public int NextLevel { get; private set; }
        public int MaxLevel => Target.ItemLevelManager.MaxLevel;

        /// <summary>
        /// Key: アイテムID, Value: 必要個数
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

        public void Open(IItemLevelable target)
        {
            Target = target;
            NextLevel = Level;
            _actorSpriteView.sprite = Target.ActorSprite;

            _levelUpCosts.Clear();
            UpdateView();

            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void ApplyLevel()
        {
            // アイテムが足りているか確認
            foreach (var c in _levelUpCosts)
            {
                var item = ItemManager.Instance.ItemSheet.GetItemData(c.Key);
                if (item.Count < c.Value)
                {
                   　// Debug.LogWarning("アイテムが足りません。");
                    return;
                }
            }

            // アイテムを消費する処理
            foreach (var c in _levelUpCosts)
            {
                var item = ItemManager.Instance.ItemSheet.GetItemData(c.Key);
                var itemData = ItemManager.Instance.ItemSheet.GetItemData(c.Key);
                itemData.Count -= c.Value;
            }

            // レベルアップ処理
            if (Level < MaxLevel)
            {
                Target.ItemLevelManager.ApplyLevel(NextLevel);
            }

            Open(Target);
        }

        public void NextLevelUp()
        {
            if (NextLevel < MaxLevel)
            {
                NextLevel++;
                // 消費アイテムの計算
                var costs = Target.ItemLevelManager.GetLevelUpCosts(NextLevel);
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
                // 消費アイテムの計算
                var costs = Target.ItemLevelManager.GetLevelUpCosts(NextLevel);
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
            // 消費アイテムの計算
            _levelUpCosts.Clear();
            for (var i = Level; i < MaxLevel; i++)
            {
                var costs = Target.ItemLevelManager.GetLevelUpCosts(i + 1);
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
            // レベル表示の更新
            _levelLabel.text = $"Lv: {Level}";
            _nextLevelLabel.text = $"Lv: {NextLevel}";
            _statusLabel.text = Target.ItemLevelManager.GetCurrentStatus().ToString();
            _nextStatusLabel.text = Target.ItemLevelManager.GetNextStatus(NextLevel).ToString();
            // 消費アイテムの表示
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