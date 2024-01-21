using System;
using System.Collections.Generic;
using UnityEngine;

namespace EquippedWeaponUI
{
    public class EquippedWindow : MonoBehaviour
    {
        [SerializeField]
        private EquippedWindowElement _equippedElementPrefab;
        [SerializeField]
        private Transform _equippedParent;

        [SerializeField]
        private EquippedWindowElement _inventoryElementPrefab;
        [SerializeField]
        private Transform _inventoryParent;

        private void Start()
        {
            var equippableCount = WeaponInventory.Current.EquippableCount;
            var inventoryCapacity = WeaponInventory.Current.InventoryCapacity;

            var equippedChangedActions = WeaponInventory.Current.OnEquippedChanged;
            var weaponCollectionChangedActions = WeaponInventory.Current.OnWeaponCollectionChanged;

            for (int i = 0; i < equippableCount; i++)
            {
                CreateElements(_equippedElementPrefab, _equippedParent, i, ref equippedChangedActions[i]);
            }
            for (int i = 0; i < inventoryCapacity; i++)
            {
                CreateElements(_inventoryElementPrefab, _inventoryParent, i, ref weaponCollectionChangedActions[i]);
            }
        }

        public void CreateElements(EquippedWindowElement prefab, Transform parent, int index, ref Action<WeaponBase> onWeaponChanged)
        {
            var element = Instantiate(prefab, parent);
            element.Initialize(index, ref onWeaponChanged);
        }
    }
}