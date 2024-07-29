using Lion.Stage;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    public class WeaponBehaviour : MonoBehaviour
    {
        public WeaponInstance Weapon { get; private set; }
        public IWeaponParameter Parameter => Weapon?.Parameter;

        public void Initialize(WeaponInstance weapon)
        {
            Weapon = weapon;
        }

        private void Start()
        {
            gameObject.SetActive(StageManager.Instance.IsBattleScene);
            StageManager.Instance.OnBattleSceneChanged += OnBattleSceneChanged;
        }

        private void OnDestroy()
        {
            StageManager.Instance.OnBattleSceneChanged -= OnBattleSceneChanged;
        }

        private void OnBattleSceneChanged(bool isBattleScene)
        {
            gameObject.SetActive(StageManager.Instance.IsBattleScene);
        }
    }
}