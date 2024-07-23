using Lion.Ally;
using System;
using UnityEngine;

namespace Lion.Formation.UI
{
    public class MinionRowManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _rows;

        private void Start()
        {
            OnActivatedAllyChanged(FormationManager.Instance.ActivatedAlly);
            FormationManager.Instance.OnActivatedAllyChanged += OnActivatedAllyChanged;
        }

        private void OnEnable()
        {
            OnActivatedAllyChanged(FormationManager.Instance.ActivatedAlly);
        }

        private void OnDestroy()
        {
            FormationManager.Instance.OnActivatedAllyChanged -= OnActivatedAllyChanged;
        }

        private void OnActivatedAllyChanged(AllyData data)
        {
            if (data == null)
            {
                foreach (var row in _rows)
                {
                    row.SetActive(false);
                }
            }
            else
            {
                UpdateButtonsActive(data.Status.AvailableMinionsCount);
            }
        }

        private void UpdateButtonsActive(int availableMinionsCount)
        {
            for (int i = 0; i < _rows.Length; i++)
            {
                if (i < availableMinionsCount)
                {
                    _rows[i].SetActive(true);
                }
                else
                {
                    _rows[i].SetActive(false);
                }
            }
        }
    }
}