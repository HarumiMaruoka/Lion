#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Glib.NovelGameEditor
{
    public class RootNodeView : NodeView, IOutputNodeView
    {
        public RootNodeView(NodeGraphView graphView, Node node, Vector2 initialPos = default) : base(node, initialPos)
        {
            _node = (RootNode)node;
            title = "Root Node";
        }

        private RootNode _node;

        public Port OutputPort => _output;

        public IOutputNode OutputConnectable => _node;
    }
}
#endif