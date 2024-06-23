#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

namespace Glib.NovelGameEditor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        private Editor _editor;

        public void ShowNodeInfomation(INodeView nodeView, Node node)
        {
            Clear();
            UnityEngine.Object.DestroyImmediate(_editor);
            if (!node) return;
            _editor = Editor.CreateEditor(node);

            IMGUIContainer container = new IMGUIContainer(() =>
            {
                _editor.OnInspectorGUI();
                if (nodeView != null && nodeView.Label != null)
                    nodeView.Label.text = node.NodeName;
            });
            Add(container);
        }
    }
}
#endif