using System;
using UnityEngine;

namespace Lion.Enemy
{
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Life { get; private set; }
        [field: SerializeField] public float Attack { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int Exp { get; private set; }
        [field: SerializeField] public int Gold { get; private set; }
        [field: SerializeField] public EnemyController Prefab { get; private set; }

        public float MoveSpeed => 1f + Speed * 0.01f;
    }
}