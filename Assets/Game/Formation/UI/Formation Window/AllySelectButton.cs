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

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenWindow);
            ApplyIcon();
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

        private void OnSelectedAlly(AllyData data)
        {
            if (!data.Unlocked) return;

            FormationManager.Instance.FrontlineAlly = data;
            ApplyIcon();
            _allySelectWindow.Close();

            _selected = data;
            OnSelected?.Invoke(data);
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