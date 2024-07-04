using Lion.Minion;
using Lion.Minion.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Formation.UI
{
    [RequireComponent(typeof(Button))]
    public class MinionSelectWindowOpenButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private int _index;
        [SerializeField] private MinionWindow _minionSelectWindow;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenWindow);
            ApplyIcon();

            if (_index < 0 || _index >= FormationManager.Instance.ActivatedMinions.Length)
            {
                Debug.LogWarning("Index is out of range.");
                return;
            }
            FormationManager.Instance.OnActivatedMinionChanged[_index] += OnActivatedMinionChanged;
        }

        private void OnDestroy()
        {
            FormationManager.Instance.OnActivatedMinionChanged[_index] -= OnActivatedMinionChanged;
        }

        private void OpenWindow()
        {
            _minionSelectWindow.Open(mode: MinionWindow.Mode.Formation);
            _minionSelectWindow.OnSelected += OnSelectedMinion;
            _minionSelectWindow.OnDisabled += OnClosedWindwo;
        }

        private void OnClosedWindwo()
        {
            _minionSelectWindow.OnSelected -= OnSelectedMinion;
            _minionSelectWindow.OnDisabled -= OnClosedWindwo;
        }

        private void OnSelectedMinion(MinionData data)
        {
            if (!data.Unlocked) return;
            if (_index < 0 || _index >= FormationManager.Instance.ActivatedMinions.Length)
            {
                Debug.LogWarning("Index is out of range.");
                return;
            }
            if (_index >= FormationManager.Instance.AvailableMinionsCount)
            {
                Debug.LogWarning("Index is out of available range.");
                return;
            }

            FormationManager.Instance.ActivatedMinions[_index] = data;
            ApplyIcon();
            _minionSelectWindow.Close();
        }

        private void ApplyIcon()
        {
            if (FormationManager.Instance.ActivatedMinions[_index] == null)
            {
                _icon.sprite = null;
                _icon.color = Color.clear;
            }
            else
            {
                _icon.sprite = FormationManager.Instance.ActivatedMinions[_index].IconSprite;
                _icon.color = Color.white;
            }
        }

        private void OnActivatedMinionChanged(MinionData data)
        {
            ApplyIcon();
        }
    }
}