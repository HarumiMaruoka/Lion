using Lion.Ally;
using Lion.Minion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Formation
{
    /// <summary>
    /// フォーメーションの管理を行うクラス。
    /// </summary>
    public class FormationManager
    {
        public static FormationManager Instance { get; private set; } = new FormationManager();

        private AllyData _activatedAlly;
        public event Action<AllyData> OnActivatedAllyChanged;

        public AllyData ActivatedAlly
        {
            get => _activatedAlly;
            set
            {
                if (value.Count == 0) return;

                if (_activatedAlly)
                    _activatedAlly.Deactivate();

                if (_activatedAlly != value) _activatedAlly = value;
                else _activatedAlly = null;

                if (_activatedAlly)
                    _activatedAlly.Activate();

                ClearMinions();
                OnActivatedAllyChanged?.Invoke(_activatedAlly);
            }
        }

        public int AvailableMinionsCount
        {
            get
            {
                if (ActivatedAlly == null) return 0;
                return ActivatedAlly.Status.AvailableMinionsCount;
            }
        }

        private MinionData[] _activatedMinions = new MinionData[4];
        public Action<MinionData>[] OnActivatedMinionChanged = new Action<MinionData>[4];
        public MinionData[] ActivatedMinions => _activatedMinions;

        public void Activation(MinionData minion, int index)
        {
            if (index < 0 || index >= _activatedMinions.Length)
            {
                Debug.LogWarning("Index is out of range.");
                return;
            }

            if (_activatedMinions[index] != null) _activatedMinions[index].Deactivate();

            if (minion == _activatedMinions[index]) _activatedMinions[index] = null;
            else _activatedMinions[index] = minion;

            if (minion != null) minion.Activate();
            OnActivatedMinionChanged[index]?.Invoke(minion);
        }

        public void Deactivation(int index)
        {
            if (index < 0 || index >= _activatedMinions.Length)
            {
                Debug.LogWarning("Index is out of range.");
                return;
            }

            if (_activatedMinions[index] != null) _activatedMinions[index].Deactivate();
            _activatedMinions[index] = null;
        }

        public void ClearMinions()
        {
            for (int i = 0; i < _activatedMinions.Length; i++)
            {
                if (_activatedMinions[i] == null) continue;
                _activatedMinions[i].Deactivate();
                _activatedMinions[i] = null;
            }
        }
    }
}