using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Gold
{
    public class GoldCollectorContainer
    {
        public static GoldCollectorContainer Instance { get; private set; } = new GoldCollectorContainer();

        private Dictionary<int, IGoldCollector> _goldCollectorByInstanceID = new Dictionary<int, IGoldCollector>();

        public void Register(GameObject gameObject, IGoldCollector goldCollector)
        {
            _goldCollectorByInstanceID[gameObject.GetInstanceID()] = goldCollector;
        }

        public void Unregister(GameObject gameObject)
        {
            _goldCollectorByInstanceID.Remove(gameObject.GetInstanceID());
        }

        public bool TryGetGoldCollector(GameObject gameObject, out IGoldCollector goldCollector)
        {
            return _goldCollectorByInstanceID.TryGetValue(gameObject.GetInstanceID(), out goldCollector);
        }
    }
}