using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.UI
{
    [DefaultExecutionOrder(-100)]
    public class ActorMessagePool : MonoBehaviour
    {
        public static ActorMessagePool Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogWarning("GameMessagePool already exists in the scene.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        [SerializeField]
        private ActorMessage _prefab;

        private HashSet<ActorMessage> _pool = new HashSet<ActorMessage>();

        public ActorMessage Get(Vector3 position = default)
        {
            foreach (var message in _pool)
            {
                if (!message.gameObject.activeSelf)
                {
                    message.gameObject.SetActive(true);
                    message.RectTransform.anchoredPosition = position;
                    return message;
                }
            }

            var newMessage = Instantiate(_prefab, transform);
            newMessage.Pool = this;
            newMessage.RectTransform.anchoredPosition = position;
            _pool.Add(newMessage);
            return newMessage;
        }

        public void Return(ActorMessage message)
        {
            message.gameObject.SetActive(false);
        }
    }
}
