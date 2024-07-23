using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    public class WeaponBehaviour : MonoBehaviour
    {
        public WeaponInstance Weapon { get; private set; }
        public IWeaponParameter Parameter { get; private set; }

        public void Initialize(WeaponInstance weapon, IWeaponParameter parameter)
        {
            Weapon = weapon;
            Parameter = parameter;
        }
    }
}