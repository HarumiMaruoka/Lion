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

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OpenWindow);

            Initialize();

            FormationManager.Instance.OnMinionChangeds[_index] += OnFrontlineMinionChanged;
        }

        private void Initialize()
        {
            _selected = FormationManager.Instance.GetFrontlineMinion(_index);
            ApplyIcon();
            OnSelected?.Invoke(_selected);
        }

        private void OnDestroy()
        {
            FormationManager.Instance.OnMinionChangeds[_index] -= OnFrontlineMinionChanged;
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

        private void OnSelectedMinion(MinionData selected)
        {
            // 未解放のミニオンが選択された場合は何もしない。
            if (!selected.Unlocked) return;
            // 既にアクティブなミニオンが選択された場合は何もしない。
            var activated = FormationManager.Instance.GetFrontlineMinion(_index);
            if (activated != selected && selected.IsActive) return;

            _selected = selected;

            FormationManager.Instance.ChangeFrontlineMinion(selected, _index);
            ApplyIcon();
            _minionSelectWindow.Close();
            OnSelected?.Invoke(selected);
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

        private void OnFrontlineMinionChanged(MinionData data)
        {
            ApplyIcon();
        }
    }
}