using System;
using UnityEngine;

namespace Lion.Ally
{
    public class AllyManager
    {
        public static AllyManager Instance { get; private set; } = new AllyManager();

        public AllySheet AllySheet { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Instance.AllySheet = ScriptableObject.Instantiate(Resources.Load<AllySheet>("AllySheet"));
            Instance.AllySheet.Initialize();
        }
    }
}