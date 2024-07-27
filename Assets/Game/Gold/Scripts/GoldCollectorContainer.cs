using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Actor
{
    public class ActorContainer
    {
        public static ActorContainer Instance { get; } = new ActorContainer();

        private Dictionary<int, IActor> _goldCollectorByInstanceID = new Dictionary<int, IActor>();

        public void Register(GameObject gameObject, IActor goldCollector)
        {
            _goldCollectorByInstanceID[gameObject.GetInstanceID()] = goldCollector;
        }

        public void Unregister(GameObject gameObject)
        {
            _goldCollectorByInstanceID.Remove(gameObject.GetInstanceID());
        }

        public bool TryGetGoldCollector(GameObject gameObject, out IActor goldCollector)
        {
            return _goldCollectorByInstanceID.TryGetValue(gameObject.GetInstanceID(), out goldCollector);
        }
    }
}