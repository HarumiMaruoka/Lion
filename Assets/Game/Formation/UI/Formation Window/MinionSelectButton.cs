using Lion.Minion;
using Lion.Minion.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Formation.UI
{
    [RequireComponent(typeof(Button))]
    public class MinionSelectButton : WeaponEquippableButton
    {
        [SerializeField] private Image _icon;
        [SerializeField] private int _index;
        [SerializeField] private MinionWindow _minionSelectWindow;
        
        private MinionData _selected;
        public override IWeaponEquippable Equippable => _selected;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenWindow);
            ApplyIcon();

            FormationManager.Instance.OnMinionChangeds[_index] += OnActivatedMinionChanged;
        }

        private void OnDestroy()
        {
            FormationManager.Instance.OnMinionChangeds[_index] -= OnActivatedMinionChanged;
        }

        private void OpenWindow()
        {
            var minion = FormationManager.Instance.GetFrontlineMinion(_index);
            _minionSelectWindow.Open(mode: MinionWindow.Mode.Formation, minion: minion);
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
            // 未解放のミニオンが選択された場合は何もしない。
            if (!data.Unlocked) return;
            // 既にアクティブなミニオンが選択された場合は何もしない。
            var activated = FormationManager.Instance.GetFrontlineMinion(_index);
            if (activated != data && data.IsActive) return;

            _selected = data;

            FormationManager.Instance.ChangeFrontlineMinion(data, _index);
            ApplyIcon();
            _minionSelectWindow.Close();
            OnSelected?.Invoke(data);
        }

        private void ApplyIcon()
        {
            if (FormationManager.Instance.GetFrontlineMinion(_index) == null)
            {
                _icon.sprite = null;
                _icon.color = Color.clear;
            }
            else
            {
                _icon.sprite = FormationManager.Instance.GetFrontlineMinion(_index).Icon;
                _icon.color = Color.white;
            }
        }

        private void OnActivatedMinionChanged(MinionData data)
        {
            ApplyIcon();
        }
    }
}