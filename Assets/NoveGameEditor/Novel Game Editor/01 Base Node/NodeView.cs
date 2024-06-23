#if UNITY_EDITOR
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Glib.NovelGameEditor
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        private Node _node;

        protected Port _input = null;
        protected Port _output = null;
        protected NodeGraphView _graphView;

        public Node Node => _node;

        public Port Input => _input;
        public Port Output => _output;

        private Label _label;
        public TextElement Label => _label;

        public event Action<INodeView, Node> OnNodeSelected;

        public NodeView(Node node, Vector2 initialPos = default)
        {
            Initialize(node, initialPos);
        }

        public NodeView(Node node, string uxml, Vector2 initialPos = default) : base(uxml)
        {
            Initialize(node, initialPos);
        }

        private void Initialize(Node node, Vector2 initialPos)
        {
            _node = node;
            this.viewDataKey = node.ViewData.GUID;

            if (initialPos != default)
            {
                _node.ViewData.Position = initialPos;
            }

            style.left = node.ViewData.Position.x;
            style.top = node.ViewData.Position.y;

            _label = this.Q<Label>("node-name");
            if (_label != null)
                _label.text = node.NodeName;

            Orientation orientation = Orientation.Horizontal;

            if (this is IInputNodeView)
            {
                Direction direction = Direction.Input;
                if (node is IMultiParent)
                {
                    _input = CreatePort(orientation, direction, Port.Capacity.Multi);
                }
                else if (node is ISingleParent)
                {
                    _input = CreatePort(orientation, direction, Port.Capacity.Single);
                }

                if (_input != null)
                {
                    inputContainer.Add(_input);
                }
            }
            if (this is IOutputNodeView)
            {
                Direction direction = Direction.Output;
                if (node is IMultiChild)
                {
                    _output = CreatePort(orientation, direction, Port.Capacity.Multi);
                }
                else if (node is ISingleChild)
                {
                    _output = CreatePort(orientation, direction, Port.Capacity.Single);
                }

                if (_output != null)
                {
                    outputContainer.Add(_output);
                }
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            _node.ViewData.Position = new Vector2(newPos.x, newPos.y);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            var nodeView = this as INodeView;
            if (nodeView != null)
                OnNodeSelected?.Invoke(nodeView, _node);
        }

        protected Port CreatePort(Orientation orientation, Direction direction, Port.Capacity capacity)
        {
            var port = Port.Create<Edge>(orientation, direction, capacity, typeof(bool));
            port.portName = "";
            return port;
        }
    }
}
#endif