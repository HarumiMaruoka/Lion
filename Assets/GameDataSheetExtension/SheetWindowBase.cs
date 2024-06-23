#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Lion.GameDataSheet
{
    public class SheetWindowBase<DataType, SheetType, WindowLayoutType> : EditorWindow
        where DataType : ScriptableObject
        where SheetType : SheetBase<DataType>
        where WindowLayoutType : WindowLayout<DataType>
    {
        private SheetBase<DataType> _sheet;
        private Vector2 _scrollPosition;
        private bool _isSettingMode = false;

        private void OnEnable()
        {
            _sheet = AssetFinder.FindAssetsByType<SheetType>();

            Undo.undoRedoPerformed += () =>
            {
                Show();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            };
        }

        void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            _sheet = EditorGUILayout.ObjectField("Target", _sheet, typeof(SheetType), false) as SheetType;
            if (_sheet)
            {
                if (!_sheet.WindowLayout)
                {
                    _sheet.WindowLayout = ScriptableObject.CreateInstance<WindowLayoutType>();
                    _sheet.WindowLayout.Sheet = _sheet;
                    AssetDatabase.AddObjectToAsset(_sheet.WindowLayout, _sheet);
                    EditorUtility.SetDirty(_sheet.WindowLayout);
                    AssetDatabase.SaveAssets();
                    _sheet.WindowLayout.name = "LayoutData";
                }

                _sheet.WindowLayout.DrawElements();

                if (GUILayout.Button("Create")) _sheet.Create();

                _sheet.WindowLayout.DrawGridLines();

                if (_isSettingMode = EditorGUILayout.Foldout(_isSettingMode, "Layout Settings"))
                {
                    _sheet.WindowLayout.DrawValueFields();
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
#endif