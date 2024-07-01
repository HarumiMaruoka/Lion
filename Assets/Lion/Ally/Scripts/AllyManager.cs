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
            Instance.AllySheet = Resources.Load<AllySheet>("AllySheet");
            Instance.AllySheet.Initialize();
        }

        private AllyData _activatedAlly;
        public AllyData ActivatedAlly
        {
            get => _activatedAlly;
            set
            {
                if (value.Count == 0) return;

                if (_activatedAlly) 
                    _activatedAlly.Deactivate();

                if (_activatedAlly != value) _activatedAlly = value;
                else _activatedAlly = null;

                if (_activatedAlly)
                    _activatedAlly.Activate();
            }
        }

    }
}