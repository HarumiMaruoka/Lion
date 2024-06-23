#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using UnityEngine;

namespace Glib.NovelGameEditor
{
    public class BranchNodeView : NodeView, IInputNodeView
    {
        public BranchNodeView(NodeGraphView graphView, Node node, Vector2 initialPos = default) :
            base(node, NovelEditorWindow.FindUxml("BranchNodeView"), initialPos)
        {
            _node = (BranchNode)node;

            title = "Branch Node";

            _elementsContainer = this.Q("elements");

            var addButton = this.Q<Button>("add-button");
            addButton.clicked += OnClickedAddElementButton;

            OnElementSelected += graphView.OnNodeSelected;

            foreach (var elem in _node.Elements)
            {
                var elementView = CreateElementView(elem);

                graphView.OnInitialized += () => graphView.CreateEdge(elem);
            }
        }

        private List<BranchElementView> _branchElementViews = new List<BranchElementView>();

        public event Action<INodeView, Node> OnElementSelected;

        private BranchNode _node;
        private VisualElement _elementsContainer;

        public Port InputPort => _input;

        public IInputNode InputConnectable => _node;

        private void OnClickedAddElementButton()
        {
            var elem = _node.CreateElement();
            CreateElementView(elem);
            base.OnSelected();
        }

        private void ElementSelected(INodeView view, Node node)
        {
            OnElementSelected?.Invoke(view, node);
            if (NodeGraphView.Current != null)
                Unselect(NodeGraphView.Current);
        }

        public BranchElementView CreateElementView(BranchElement element)
        {
            var elemView = new BranchElementView(_graphView, element);

            _branchElementViews.Add(elemView);
            _elementsContainer.Add(elemView);

            elemView.OnNodeSelected += ElementSelected;
            elemView.OnClickedRemoveButton += DeleteElementView;

            return elemView;
        }

        public void DeleteElementView(BranchElementView elementView)
        {
            _node.DeleteElement(elementView.Node);
            _elementsContainer.Remove(elementView);
        }

        public void DeleteElements()
        {
            foreach (var elemView in _branchElementViews)
            {
                DeleteElementView(elemView);
            }
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
        }
    }
}
#endif