using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lion.Ally.UI
{
    public class AllyWindow : MonoBehaviour
    {
        [SerializeField] private Row _rowPrefab;
        [SerializeField] private Transform _rowParent;

        private List<Row> _rows = new List<Row>();

        public Action<AllyData> OnSelectedBuffer;
        public event Action<AllyData> OnSelected
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

        private IEnumerable<AllyData> _last;

        public void Open(IEnumerable<AllyData> allies = null, Mode mode = Mode.Normal)
        {
            if (allies == null) allies = AllyManager.Instance.AllySheet;

            gameObject.SetActive(true);
            if (_last == allies)
            {
                UpdateIcons(mode);
            }
            else
            {
                UpdateIcons(allies, mode);
                _last = allies;
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private int CalculateRowCount()
        {
            var maxAllyCount = AllyManager.Instance.AllySheet.Count;
            return Mathf.CeilToInt((float)maxAllyCount / 2f);
        }

        public AllyIcon GetIcon(int index)
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

        public void UpdateIcons(IEnumerable<AllyData> allies, Mode mode)
        {
            var count = allies.Count();
            // 既存のアイコンに対して、新しいデータを割り当てる。
            var index = 0;
            foreach (var ally in allies)
            {
                var icon = GetIcon(index);
                icon.IsFormationMode = mode == Mode.Formation;
                icon.Ally = ally;
                index++;
            }
            // 残りのアイコンに対して、nullを割り当てる。
            for (int i = count; i < AllyManager.Instance.AllySheet.Count; i++)
            {
                var icon = GetIcon(i);
                icon.Ally = null;
            }
        }

        public void UpdateIcons(Mode mode)
        {
            foreach (var row in _rows)
            {
                row.Left.IsFormationMode = mode == Mode.Formation;
                row.Right.IsFormationMode = mode == Mode.Formation;
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
