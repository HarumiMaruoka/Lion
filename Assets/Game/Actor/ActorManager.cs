using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Actor
{
    public class ActorManager
    {
        private static Dictionary<int, IActor> _actorDict = new Dictionary<int, IActor>();

        public static bool TryGetActor(GameObject gameObject, out IActor actor)
        {
            var instanceId = gameObject.GetInstanceID();
            return _actorDict.TryGetValue(instanceId, out actor);
        }

        public static void Register(IActor actor)
        {
            var instanceId = actor.gameObject.GetInstanceID();
            _actorDict[instanceId] = actor;
        }

        public static void Unregister(IActor actor)
        {
            var instanceId = actor.gameObject.GetInstanceID();
            _actorDict.Remove(instanceId);
        }
    }
}