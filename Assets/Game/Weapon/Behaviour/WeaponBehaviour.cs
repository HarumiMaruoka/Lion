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
    }
}