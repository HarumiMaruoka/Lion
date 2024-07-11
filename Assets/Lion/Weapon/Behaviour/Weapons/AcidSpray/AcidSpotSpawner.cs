using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.AcidSprayModule
{
    public class AcidSpotSpawner : MonoBehaviour
    {
        [SerializeField]
        private SprayEffect _spray;

        public void CreateAcidSpot()
        {
            _spray.CreateAcidSpot();
        }
    }
}