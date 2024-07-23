using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lion.Minion.UI
{
    public class MinionWindow : MonoBehaviour
    {
        [SerializeField] private Row _rowPrefab;
        [SerializeField] private Transform _rowParent;

        private List<Row> _rows = new List<Row>();

        public Action<MinionData> OnSelectedBuffer;
        public event Action<MinionData> OnSelected
        {
            add
            {
                OnSelectedBuffer += value;
                foreach (var row in _rows)
                {
                    row.Left.OnSelected += value;
                    row.Right.OnSelected += value;
                }
            }
            remove
            {
                OnSelectedBuffer -= value;
                foreach (var row in _rows)
                {
                    row.Left.OnSelected -= value;
                    row.Right.OnSelected -= value;
                }
            }
        }

        private void Awake()
        {
            CreateRowInstances();
        }

        private IEnumerable<MinionData> _last;

        public void Open(IEnumerable<MinionData> minions = null, Mode mode = Mode.Normal, MinionData minion = null)
        {
            if (minions == null) minions = MinionManager.Instance.MinionSheet;

            gameObject.SetActive(true);
            if (_last == minions)
            {
                UpdateIcons(mode, minion);
            }
            else
            {
                UpdateIcons(minions, mode, minion);
                _last = minions;
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private int CalculateRowCount()
        {
            var maxAllyCount = MinionManager.Instance.MinionSheet.Count;
            return Mathf.CeilToInt((float)maxAllyCount / 2f);
        }

        public MinionIcon GetIcon(int index)
        {
            var row = _rows[index / 2];
            return index % 2 == 0 ? row.Left : row.Right;
        }

        private void CreateRowInstances()
        {
            for (int i = 0; i < CalculateRowCount(); i++)
            {
                var row = Instantiate(_rowPrefab, _rowParent);
                row.Left.OnSelected += OnSelectedBuffer;
                row.Right.OnSelected += OnSelectedBuffer;
                _rows.Add(row);
            }
        }

        public void UpdateIcons(IEnumerable<MinionData> allies, Mode mode, MinionData minion)
        {
            var count = allies.Count();
            // 既存のアイコンに対して、新しいデータを割り当てる。
            var index = 0;
            foreach (var ally in allies)
            {
                var icon = GetIcon(index);
                icon.IsFormationMode = mode == Mode.Formation;
                icon.Minion = ally;
                icon.FormationModeMinion = minion;
                index++;
            }
            // 残りのアイコンに対して、nullを割り当てる。
            for (int i = count; i < MinionManager.Instance.MinionSheet.Count; i++)
            {
                var icon = GetIcon(i);
                icon.Minion = null;
            }
        }

        public void UpdateIcons(Mode mode, MinionData minion)
        {
            foreach (var row in _rows)
            {
                row.Left.IsFormationMode = mode == Mode.Formation;
                row.Right.IsFormationMode = mode == Mode.Formation;

                row.Left.FormationModeMinion = minion;
                row.Right.FormationModeMinion = minion;

                row.UpdateUI();
            }
        }

        public event Action OnDisabled;
        private void OnDisable()
        {
            OnDisabled?.Invoke();
        }

        public enum Mode
        {
            Normal,
            Formation
        }
    }
}