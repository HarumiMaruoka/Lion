using System;
using UnityEngine;

namespace Glib.NovelGameEditor
{
    public class NovelRunner : MonoBehaviour
    {
        [SerializeField]
        private Config _config;

        private Node _current = null;

        public Config Config => _config;

        public event Action OnStarted;
        public event Action OnStopped;

        public void Play(NovelNodeGraph _nodeGraph)
        {
            if (!_nodeGraph)
            {
                Debug.LogWarning("Warning: NovelNodeGraph not assigned.");
                return;
            }

            foreach (var node in _nodeGraph.Nodes)
            {
                node.Initialize(this);
            }

            _current = _nodeGraph.RootNode.Child;
            _current.OnEnter();

            OnStarted?.Invoke();
        }

        public void Stop()
        {
            _current = null;

            OnStopped?.Invoke();
        }

        private void Update()
        {
            if (_current != null) _current.OnUpdate();
        }

        public void MoveTo(Node node)
        {
            _current.OnExit();
            _current = node;
            if (_current) _current.OnEnter();
            else OnStopped?.Invoke();
        }
    }
}