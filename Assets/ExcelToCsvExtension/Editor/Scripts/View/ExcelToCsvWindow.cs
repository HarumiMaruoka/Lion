using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GL
{
    public class ExcelToCsvWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("Window/Excel To Csv")]
        public static void ShowExample()
        {
            ExcelToCsvWindow wnd = GetWindow<ExcelToCsvWindow>();
            wnd.titleContent = new GUIContent("Excel To Csv");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
            root.Add(labelFromUXML);

            // Initialize
            UIManager uiManager = new UIManager();
            uiManager.Bind(labelFromUXML);
        }
    }
}