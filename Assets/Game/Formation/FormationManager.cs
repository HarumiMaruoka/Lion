using Lion.Ally;
using Lion.Minion;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lion.Formation
{
    /// <summary>
    /// フォーメーションの管理を行うクラス。
    /// </summary>
    public class FormationManager
    {
        public static FormationManager Instance { get; private set; } = new FormationManager();

        private AllyData _frontlineAlly;
        private MinionData[] _frontlineMinions = new MinionData[4];

        public event Action<AllyData> OnAllyChanged;
        public Action<MinionData>[] OnMinionChangeds = new Action<MinionData>[4];
        public Action<int, MinionData> OnMinionChanged;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded += Instance.OnSceneLoaded;
        }

        public float BattlePower
        {
            get
            {
                float battlePower = 0f;
                if (FrontlineAlly != null) battlePower += FrontlineAlly.Status.BattlePower;
                foreach (var minion in _frontlineMinions)
                {
                    if (minion != null) battlePower += minion.Status.BattlePower;
                }
                return battlePower;
            }
        }

        public AllyData FrontlineAlly
        {
            get => _frontlineAlly;
            set
            {
                if (value.Count == 0) return;

                _frontlineAlly?.Deactivate();

                // 既に選択されているアクターが選択された場合は、選択を解除する操作とする。
                _frontlineAlly = _frontlineAlly != value ? value : null;

                _frontlineAlly?.Activate();

                ClearMinions();
                OnAllyChanged?.Invoke(_frontlineAlly);
            }
        }

        public MinionData GetFrontlineMinion(int index)
        {
            if (index < 0 || index >= _frontlineMinions.Length)
            {
                Debug.LogWarning("Index is out of range.");
                return null;
            }

            return _frontlineMinions[index];
        }

        public void ChangeFrontlineMinion(MinionData next, int index)
        {
            if (index < 0 || index >= _frontlineMinions.Length)
            {
                Debug.LogWarning("Index is out of range.");
                return;
            }

            _frontlineMinions[index]?.Deactivate();

            _frontlineMinions[index] = _frontlineMinions[index] != next ? next : null;

            _frontlineMinions[index]?.Activate();

            OnMinionChangeds[index]?.Invoke(_frontlineMinions[index]);
            OnMinionChanged?.Invoke(index, _frontlineMinions[index]);
        }

        public void ClearMinions()
        {
            for (int i = 0; i < _frontlineMinions.Length; i++)
            {
                if (_frontlineMinions[i] == null) continue;
                _frontlineMinions[i].Deactivate();
                _frontlineMinions[i] = null;
                OnMinionChangeds[i]?.Invoke(null);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (_frontlineAlly) _frontlineAlly.Activate();
            foreach (var minion in _frontlineMinions)
            {
                if (minion) minion.Activate();
            }
        }
    }
}