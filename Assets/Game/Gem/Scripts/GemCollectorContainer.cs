using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lion.Gem
{
    public class GemCollectorContainer
    {
        public static GemCollectorContainer Instance { get; private set; } = new GemCollectorContainer();

        private Dictionary<int, IGemCollector> _gemCollectorByInstanceID = new Dictionary<int, IGemCollector>();

        public void Register(GameObject gameObject, IGemCollector gemCollector)
        {
            _gemCollectorByInstanceID[gameObject.GetInstanceID()] = gemCollector;
        }

        public void Unregister(GameObject gameObject)
        {
            _gemCollectorByInstanceID.Remove(gameObject.GetInstanceID());
        }

        public bool TryGetGemCollector(GameObject gameObject, out IGemCollector gemCollector)
        {
            return _gemCollectorByInstanceID.TryGetValue(gameObject.GetInstanceID(), out gemCollector);
        }
    }
}