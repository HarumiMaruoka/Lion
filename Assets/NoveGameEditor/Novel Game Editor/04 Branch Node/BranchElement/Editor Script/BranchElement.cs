using System;
using UnityEngine;

namespace Glib.NovelGameEditor
{
    public class BranchElement : Node, ISingleChild
    {
#if Novel_Game_Editor_Development
        [SerializeField]
#else
        [SerializeField]
        [HideInInspector]
#endif
        private Node _child;
        [SerializeField]
        private string _label;

        public string Label => _label;

        public Node Child { get => _child; set => _child = value; }

        public Node Node => this;

#if UNITY_EDITOR
        public override Type NodeViewType => typeof(BranchNodeView);
#endif

        public override void Initialize(NovelRunner controller)
        {

        }

        public void OutputConnect(Node child)
        {
            _child = child;
        }

        public void OutputDisconnect(Node child)
        {
            _child = null;
        }
    }
}