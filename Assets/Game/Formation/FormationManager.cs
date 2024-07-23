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

        public float BattlePower
        {
            get
            {
                float battlePower = 0;
                if (ActivatedAlly != null) battlePower += ActivatedAlly.Status.BattlePower;
                for (int i = 0; i < _activatedMinions.Length; i++)
                {
                    if (_activatedMinions[i] == null) continue;
                    battlePower += 1f; // _activatedMinions[i].Status.BattlePower;
                }
                return battlePower;
            }
        }

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

        public int ActivatableMinionsCount => _activatedMinions.Length;

        public MinionData GetActivatedMinion(int index)
        {
            if (index < 0 || index >= _activatedMinions.Length)
            {
                Debug.LogWarning("Index is out of range.");
                return null;
            }

            return _activatedMinions[index];
        }

        public void SetActivatedMinion(MinionData next, int index)
        {
            if (index < 0 || index >= _activatedMinions.Length)
            {
                Debug.LogWarning("Index is out of range.");
                return;
            }

            var old = _activatedMinions[index];
            if (old != null) old.Deactivate();

            if (old == next) _activatedMinions[index] = null;
            else _activatedMinions[index] = next;

            if (_activatedMinions[index] != null) _activatedMinions[index].Activate();

            OnActivatedMinionChanged[index]?.Invoke(_activatedMinions[index]);
        }

        public void ClearMinions()
        {
            for (int i = 0; i < _activatedMinions.Length; i++)
            {
                if (_activatedMinions[i] == null) continue;
                _activatedMinions[i].Deactivate();
                _activatedMinions[i] = null;
                OnActivatedMinionChanged[i]?.Invoke(null);
            }
        }
    }
}