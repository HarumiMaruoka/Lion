using Lion.Ally;
using Lion.Ally.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Formation.UI
{
    [RequireComponent(typeof(Button))]
    public class AllySelectButton : WeaponEquippableButton
    {
        [SerializeField] private Image _icon;
        [SerializeField] private AllyWindow _allySelectWindow;

        private AllyData _selected;
        public override IWeaponEquippable Equippable => _selected;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OpenWindow);
            Initialize();
        }

        private void Initialize()
        {
            _selected = FormationManager.Instance.FrontlineAlly;
            ApplyIcon();
            OnSelected?.Invoke(_selected);
        }

        private void OpenWindow()
        {
            _allySelectWindow.Open(mode: AllyWindow.Mode.Formation);
            _allySelectWindow.OnSelected += OnSelectedAlly;
            _allySelectWindow.OnDisabled += OnClosedWindwo;
        }

        private void OnClosedWindwo()
        {
            _allySelectWindow.OnSelected -= OnSelectedAlly;
            _allySelectWindow.OnDisabled -= OnClosedWindwo;
        }

        private void OnSelectedAlly(AllyData selected)
        {
            if (!selected.Unlocked) return;

            FormationManager.Instance.FrontlineAlly = selected;
            ApplyIcon();
            _allySelectWindow.Close();

            _selected = selected;
            OnSelected?.Invoke(selected);
        }

        private void ApplyIcon()
        {
            if (FormationManager.Instance.FrontlineAlly == null)
            {
                _icon.sprite = null;
                _icon.color = Color.clear;
            }
            else
            {
                _icon.sprite = FormationManager.Instance.FrontlineAlly.Icon;
                _icon.color = Color.white;
            }
        }
    }
}