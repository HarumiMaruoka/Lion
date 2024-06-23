#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Glib.NovelGameEditor
{
    public class BranchElementView : VisualElement, IOutputNodeView
    {
        private BranchElement _node;
        private Port _output = null;

        public event Action<INodeView, Node> OnNodeSelected;

        public BranchElementView(NodeGraphView graphView, BranchElement node, Vector2 initialPos = default)
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
                (NovelEditorWindow.FindUxml("BranchElementView"));
            visualTree.CloneTree(this);

            _node = node;

            this.viewDataKey = node.ViewData.GUID;
            Direction direction = Direction.Output;

            _output = CreatePort(direction, Port.Capacity.Single);
            var outputContainer = this.Q("output");
            outputContainer.Add(_output);

            _mainButton = this.Q<Button>("main");
            _mainButton.clicked += Select;
            _mainButton.text = "Branch Element";
            _mainButton.text = node.NodeName;
            this.Q<Button>("remove").clicked += ClickedRemoveButton;
        }

        private Button _mainButton;
        public BranchElement Node => _node;
        public Port OutputPort => _output;
        public IOutputNode OutputConnectable => _node;

        public TextElement Label => _mainButton;

        public event Action<BranchElementView> OnClickedRemoveButton;

        private Port CreatePort(Direction direction, Port.Capacity capacity)
        {
            var port = Port.Create<Edge>(Orientation.Horizontal, direction, capacity, typeof(bool));
            port.portName = "";
            return port;
        }

        private void Select()
        {
            OnNodeSelected?.Invoke(this, _node);
        }

        private void ClickedRemoveButton()
        {
            OnClickedRemoveButton?.Invoke(this);
        }
    }
}
#endif