using Lion.Ally.Skill;
using Lion.Formation;
using Lion.Player;
using Lion.Weapon;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public class AllyData : ScriptableObject, IWeaponEquippable
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite ActorSprite { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public AllyController Prefab { get; private set; }
        [field: SerializeField] public SkillBase SkillPrefab { get; private set; }

        private AllyController _instance;
        private int _count; // 所持数
        private WeaponInstance[] _equipped = new WeaponInstance[4];

        public event Action<bool> OnActiveChanged;
        public event Action<int> OnCountChanged;
        public event Action<bool> OnUnlockStatusChanged;

        public AllyLevelManager LevelManager { get; private set; }
        public bool IsActive => _instance != null;
        public bool Unlocked => _count > 0;
        public AllyStatus Status => LevelManager.Status;
        public AllyController Instance => _instance;

        public int Count
        {
            get => _count;
            set
            {
                if (_count == 0 && value > 0)
                {
                    OnUnlockStatusChanged?.Invoke(true);
                }
                else if (_count > 0 && value == 0)
                {
                    OnUnlockStatusChanged?.Invoke(false);
                }

                _count = value;
                OnCountChanged?.Invoke(value);
            }
        }

        public void Initialize()
        {
            Count = 0;
            LevelManager = new AllyLevelManager(this);
        }

        public void Activate()
        {
            _instance = Instantiate(Prefab, PlayerController.Instance.transform.position, Quaternion.identity);
            _instance.AllyData = this;
            for (int i = 0; i < _equipped.Length; i++)
            {
                _equipped[i]?.Activation(_instance);
            }
            OnActiveChanged?.Invoke(true);
        }

        public void Deactivate()
        {
            Destroy(_instance.gameObject);
            _instance = null;
            ClearWeapon();
            OnActiveChanged?.Invoke(false);
        }

        private void ClearWeapon()
        {
            foreach (var weapon in _equipped)
            {
                weapon?.Deactivation();
            }
        }

        public WeaponInstance Equipped(int index)
        {
            return _equipped[index];
        }

        public void Equip(WeaponInstance weapon, int index)
        {
            if (index < 0 || index >= _equipped.Length)
            {
                Debug.LogWarning("Index is out of range.");
                return;
            }

            _equipped[index]?.Deactivation();
            _equipped[index] = weapon == _equipped[index] ? null : weapon;
            _equipped[index]?.Activation(_instance);
        }
    }
}