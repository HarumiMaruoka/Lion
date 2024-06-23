#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Glib.NovelGameEditor
{
    public class NovelEditorWindow : EditorWindow
    {
        private InspectorView _inspectorView = null;
        private NodeGraphView _nodeGraphView = null;
        private Label _invalidedLabel = null;

        [MenuItem("Window/UI Toolkit/NovelEditorWindow")]
        public static void OpenWindow()
        {
            NovelEditorWindow wnd = GetWindow<NovelEditorWindow>();
            wnd.titleContent = new GUIContent("NovelEditorWindow");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            var uxmlPath = FindUxml("NovelEditorWindow");
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            visualTree.CloneTree(root);

            var ussPath = FindUss("NovelEditorWindow");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
            root.styleSheets.Add(styleSheet);

            _nodeGraphView = root.Q<NodeGraphView>();
            _inspectorView = root.Q<InspectorView>();
            _invalidedLabel = root.Q<Label>("invalided-label");

            _nodeGraphView.OnNodeSelectedEvent += OnNodeSelected;
            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            var selectGameObject = UnityEditor.Selection.activeObject as NovelNodeGraph;

            if (selectGameObject)
            {
                selectGameObject.OnDestroyed += _ => OnSelectionChange();
                _nodeGraphView.PopulateView(selectGameObject);
                _invalidedLabel.style.opacity = 0f; // 非有効時のメッセージを隠す。
                return;
            }

            //_invalidedLabel.style.opacity = 1f; // 非有効時のメッセージを表示する。
            //_inspectorView.ShowNodeInformation(null, null);
            //_nodeGraphView.PopulateView(null);
        }

        public void OnNodeSelected(INodeView nodeView, Node node)
        {
            _inspectorView.ShowNodeInfomation(nodeView, node);
        }

        public static string FindUxml(string name)
        {
            return Find(name, "uxml");
        }

        public static string FindUss(string name)
        {
            return Find(name, "uss");
        }

        private static string Find(string name, string extension)
        {
            var assetPaths = AssetDatabase.FindAssets(name)
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Where(path => Path.GetExtension(path) == "." + extension)
                .ToArray();

            if (assetPaths.Length == 0)
            {
                Debug.LogWarning($"\"{name}\"の{extension}ファイルが見つかりませんでした。");
                return null;
            }
            else if (assetPaths.Length > 1)
            {
                Debug.LogWarning($"\"{name}\"で{assetPaths.Length}件の結果が見つかりました。\n\n{string.Join("\n", assetPaths)}");
            }

            return assetPaths[0];
        }
    }
}
#endif