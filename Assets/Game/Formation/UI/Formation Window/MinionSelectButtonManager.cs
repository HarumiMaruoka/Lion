using Lion.Ally;
using System;
using UnityEngine;

namespace Lion.Formation.UI
{
    public class MinionSelectButtonManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _rows;

        private void Start()
        {
            OnFrontlineAllyChanged(FormationManager.Instance.FrontlineAlly);
            FormationManager.Instance.OnAllyChanged += OnFrontlineAllyChanged;
        }

        private void OnEnable()
        {
            OnFrontlineAllyChanged(FormationManager.Instance.FrontlineAlly);
        }

        private void OnDestroy()
        {
            FormationManager.Instance.OnAllyChanged -= OnFrontlineAllyChanged;
        }

        private void OnFrontlineAllyChanged(AllyData ally)
        {
            if (ally == null)
            {
                foreach (var row in _rows)
                {
                    row.SetActive(false);
                }
            }
            else
            {
                RefreshRowStates(ally.Status.AvailableMinionsCount);
            }
        }

        private void RefreshRowStates(int availableMinionsCount)
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