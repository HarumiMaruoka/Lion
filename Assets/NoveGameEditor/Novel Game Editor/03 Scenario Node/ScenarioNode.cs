using Glib.NovelGameEditor.Scenario;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glib.NovelGameEditor
{
    [GraphViewContextMenu]
    public class ScenarioNode : Node, IMultiParent, ISingleChild
    {
        #region Parent Child Connection

#if UNITY_EDITOR
        public override Type NodeViewType => typeof(ScenarioNodeView);
#endif
        public Node Node => this;

        [SerializeField]
        [HideInInspector]
        private List<Node> _parents = new List<Node>();
        public List<Node> Parents => _parents;

        [SerializeField]
        [HideInInspector]
        private Node _child;
        public Node Child { get => _child; set => _child = value; }

        public void InputConnect(Node parent)
        {
            _parents.Add(parent);
        }

        public void InputDisconnect(Node parent)
        {
            _parents.Remove(parent);
        }

        public void OutputConnect(Node child)
        {
            Child = child;
        }

        public void OutputDisconnect(Node child)
        {
            Child = null;
        }
        #endregion

        [SerializeField] private ScenarioRunner _commandRunner;

        public override void Initialize(NovelRunner controller)
        {
            _controller = controller;
        }

        public override void OnEnter()
        {
            _commandRunner.RunScenario(_controller.Config);
        }

        public override void OnUpdate()
        {
            if (_commandRunner.IsFinished)
            {
                _controller.MoveTo(Child);
            }
        }

        public override void OnExit()
        {

        }
    }
}