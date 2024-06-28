using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Ally.UI
{
    public class AllyWindow : MonoBehaviour
    {
        [SerializeField] private Row _rowPrefab;
        [SerializeField] private Transform _rowPrent;

        private List<Row> _rows = new List<Row>();

        public event Action<AllyData> OnSelected
        {
            add
            {
                foreach (var row in _rows)
                {
                    row.Left.OnSelected += value;
                    row.Right.OnSelected += value;
                }
            }
            remove
            {
                foreach (var row in _rows)
                {
                    row.Left.OnSelected -= value;
                    row.Right.OnSelected -= value;
                }
            }
        }

        private void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            UpdateUI();
        }

        public int CalculateRowCount()
        {
            return Mathf.CeilToInt((float)AllyManager.Instance.AllySheet.Count / 2);
        }

        public AllyIcon GetAllyIcon(int index)
        {
            var row = _rows[index / 2];
            return index % 2 == 0 ? row.Left : row.Right;
        }

        public void Initialize()
        {
            for (int i = 0; i < CalculateRowCount(); i++)
            {
                var row = Instantiate(_rowPrefab, _rowPrent);
                _rows.Add(row);
            }

            for (int i = 0; i < AllyManager.Instance.AllySheet.Count; i++)
            {
                var ally = AllyManager.Instance.AllySheet[i];
                var icon = GetAllyIcon(i);
                icon.Ally = ally;
            }
        }

        public void UpdateUI()
        {
            foreach (var row in _rows)
            {
                row.UpdateUI();
            }
        }
    }
}
