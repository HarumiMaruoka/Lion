#if UNITY_EDITOR
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Glib.NovelGameEditor
{
    public class ScenarioNodeView : NodeView, IOutputNodeView, IInputNodeView
    {
        public ScenarioNodeView(NodeGraphView graphView, Node node, Vector2 initialPos = default) :
            base(node, NovelEditorWindow.FindUxml("ScenarioNodeView"), initialPos)
        {
            _node = (ScenarioNode)node;
            title = "Scenario Node";
        }

        private ScenarioNode _node;

        public Port OutputPort => _output;

        public IOutputNode OutputConnectable => _node;

        public Port InputPort => _input;

        public IInputNode InputConnectable => _node;
    }
}
#endif