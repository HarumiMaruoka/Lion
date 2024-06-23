#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Glib.NovelGameEditor
{
    public class NodeGraphView : GraphView
    {
        private static NodeGraphView _current = null;
        public static NodeGraphView Current => _current;

        public new class UxmlFactory : UxmlFactory<NodeGraphView, GraphView.UxmlTraits> { }

        private NovelNodeGraph _nodeGraph;

        public NovelNodeGraph NodeGraph => _nodeGraph;

        public NodeGraphView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(NovelEditorWindow.FindUss("NodeGraphView"));
            this.styleSheets.Add(styleSheet);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var types = TypeCache.GetTypesDerivedFrom<Glib.NovelGameEditor.Node>();
            foreach (var type in types)
            {
                var contextMenuAttribute = Attribute.GetCustomAttribute(type, typeof(GraphViewContextMenuAttribute));
                if (contextMenuAttribute == null) continue;

                var mousePos = GetMousePosition(evt);

                evt.menu.AppendAction($"{type.Name}", _ =>
                {
                    var node = _nodeGraph.CreateNode(type);
                    CreateNodeView(node, mousePos);
                });
            }
        }

        public event Action OnInitialized;

        public void PopulateView(NovelNodeGraph graph)
        {
            OnInitialized = null;
            _current = null;
            ClearNodeGraphView();
            if (graph == null) return;

            _current = this;
            _nodeGraph = graph;
            _nodeGraph.OnDestroyed += _ => ClearNodeGraphView();

            graphViewChanged += OnGraphViewChanged;

            if (_nodeGraph.RootNode == null) // ルートノードが無ければ作成する。
            {
                _nodeGraph.RootNode = _nodeGraph.CreateNode(typeof(RootNode)) as RootNode;
                AssetDatabase.SaveAssets();
            }

            // Create Node view
            foreach (var node in _nodeGraph.Nodes)
            {
                CreateNodeView(node, default);
            }

            // Create edges
            foreach (var node in _nodeGraph.Nodes)
            {
                var singleChild = node as ISingleChild;
                if (singleChild != null)
                    CreateEdge(singleChild);

                var multiChild = node as IMultiChild;
                if (multiChild != null)
                    CreateEdge(multiChild);
            }
            OnInitialized?.Invoke();
        }

        private void CreateNodeView(Node node, Vector2 initialPos)
        {
            var nodeViewType = node.NodeViewType;

            NodeView nodeView = null;
            var constructor = nodeViewType.GetConstructor(new[] { typeof(NodeGraphView), typeof(Node), typeof(Vector2) });
            if (constructor != null)
            {
                var instance = constructor.Invoke(new object[] { this, node, initialPos });
                nodeView = (NodeView)instance;
            }
            else
            {
                throw new MissingMethodException();
            }

            if (nodeView != null)
            {
                this.AddElement(nodeView);
                nodeView.OnNodeSelected += OnNodeSelected;
            }
        }

        public void ClearNodeGraphView()
        {
            _nodeGraph = null;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
        }

        public void CreateEdge(ISingleChild parent)
        {
            // 子と親を繋げる。
            var child = parent.Child;
            if (child == null) return;

            Port outputPort = GetOutputPort(parent.Node);
            Port inputPort = GetInputPort(child);

            var edge = outputPort.ConnectTo(inputPort);
            AddElement(edge);
        }

        private void CreateEdge(IMultiChild parent)
        {
            foreach (var child in parent.Children)
            {
                if (child == null) continue;

                Port outputPort = GetOutputPort(parent.Node);
                Port inputPort = GetInputPort(child);

                var edge = outputPort.ConnectTo(inputPort);
                AddElement(edge);
            }
        }

        private NodeView FindNodeView(Glib.NovelGameEditor.Node node)
        {
            var nodeViews = this.Query<NodeView>().ToList();

            foreach (var nodeView in nodeViews)
            {
                if (nodeView.viewDataKey == node.ViewData.GUID)
                {
                    return nodeView;
                }
            }

            return null;
        }

        private BranchElementView FindElementView(BranchElement element)
        {
            var branchNodeViews = this.Query<BranchNodeView>().ToList();

            foreach (var nodeView in branchNodeViews)
            {
                var elemViews = nodeView.Query<BranchElementView>().ToList();

                foreach (var elemView in elemViews)
                {
                    if (elemView.viewDataKey == element.ViewData.GUID)
                    {
                        return elemView;
                    }
                }
            }

            return null;
        }

        private Port GetInputPort(Glib.NovelGameEditor.Node node)
        {
            return FindNodeView(node).Input;
        }

        private Port GetOutputPort(Glib.NovelGameEditor.Node node)
        {
            if (node is BranchElement)
            {
                var view = FindElementView(node as BranchElement);
                return view.OutputPort;
            }

            return FindNodeView(node).Output;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.edgesToCreate != null)
            {
                OnCreatedEdge(graphViewChange);
            }

            if (graphViewChange.elementsToRemove != null)
            {
                OnRemovedElements(graphViewChange);
            }

            EditorUtility.SetDirty(_nodeGraph);

            return graphViewChange;
        }

        private void OnCreatedEdge(GraphViewChange graphViewChange)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                IOutputNodeView parentView = edge.output.GetFirstAncestorOfType<IOutputNodeView>();
                IInputNodeView childView = edge.input.GetFirstAncestorOfType<IInputNodeView>();

                _nodeGraph.Connect(parentView, childView);
            });
        }

        public void OnDeleteEdge(Edge edge) // TODO このメソッドの呼び出し
        {
            IOutputNodeView parentView = edge.output.GetFirstAncestorOfType<IOutputNodeView>();
            IInputNodeView childView = edge.input.GetFirstAncestorOfType<IInputNodeView>();

            parentView.OutputConnectable.OutputDisconnect(childView.InputConnectable.Node);
            childView.InputConnectable.InputDisconnect(parentView.OutputConnectable.Node);
        }

        private void OnRemovedElements(GraphViewChange graphViewChange)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    var branchNodeView = nodeView as BranchNodeView;
                    if (branchNodeView != null)
                    {
                        branchNodeView.DeleteElements();
                    }
                    _nodeGraph.DeleteNode(nodeView.Node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    OnDeleteEdge(edge);
                }
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
            endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
        }

        public event Action<INodeView, Glib.NovelGameEditor.Node> OnNodeSelectedEvent;

        public void OnNodeSelected(INodeView nodeView, Glib.NovelGameEditor.Node node)
        {
            OnNodeSelectedEvent(nodeView, node);
        }

        public Vector2 GetMousePosition(ContextualMenuPopulateEvent evt)
        {
            var mousePos = evt.localMousePosition;
            var worldPos = ElementAt(0).LocalToWorld(mousePos);
            var localPos = ElementAt(1).WorldToLocal(worldPos);

            return localPos;
        }
    }
}
#endif