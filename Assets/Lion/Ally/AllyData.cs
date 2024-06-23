using System;
using UnityEngine;

namespace Lion.Ally
{
    public class AllyData : ScriptableObject
    {
        [SerializeField]
        private int _id;
        [SerializeField]
        private string _name;

        public int ID => _id;
        public string Name => _name;
    }
}