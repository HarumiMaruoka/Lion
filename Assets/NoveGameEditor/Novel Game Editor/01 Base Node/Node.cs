using System;
using UnityEngine;

namespace Glib.NovelGameEditor
{
    public abstract class Node : ScriptableObject
    {
#if Novel_Game_Editor_Development
        [SerializeField]
#else
        [SerializeField]
        [HideInInspector]
#endif
        private NodeViewData _viewData;
        [SerializeField]
        protected string _nodeName;

        protected NovelRunner _controller;

#if UNITY_EDITOR
        public abstract Type NodeViewType { get; }
#endif

        public NodeViewData ViewData => _viewData ??= new NodeViewData();
        public string NodeName => _nodeName;
        public NovelRunner Controller => _controller;

        public virtual void Initialize(NovelRunner controller)
        {
            _controller = controller;
        }

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
    }
}