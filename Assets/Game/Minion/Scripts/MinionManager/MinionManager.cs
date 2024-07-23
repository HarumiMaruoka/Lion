using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Minion
{
    public class MinionManager
    {
        public static MinionManager Instance { get; private set; } = new MinionManager();

        public MinionSheet MinionSheet { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Instance.MinionSheet = Resources.Load<MinionSheet>("MinionSheet");
            Instance.MinionSheet.Initialize();
        }
    }
}