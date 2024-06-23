#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Lion.GameDataSheet
{
    public class WindowLayout<T> : ScriptableObject where T : ScriptableObject
    {
        [field: SerializeReference] public SheetBase<T> Sheet { get; set; }

        [SerializeField] public List<float> HorizontalSpacings = new List<float>();
        [SerializeField] public List<float> VerticalSpacings = new List<float>();

        [SerializeField] public Color GridLineColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        [SerializeField] public Vector2 GridLineSize = new Vector2(3f, 2f);
        [SerializeField] public Vector2 GridLineOffset = new Vector2(2f, 21f);

        // 指定間隔で水平方向に特定のSerializedObjectの要素を描画する。
        private void DrawElement(T element, float height)
        {
            var serializedElement = new SerializedObject(element);
            var heightOption = GUILayout.Height(height);

            var it = serializedElement.GetIterator();
            it.NextVisible(true);

            serializedElement.Update();
            int counter = 0;
            while (it.NextVisible(false))
            {
                if (counter == HorizontalSpacings.Count)
                {
                    var guiContent = new GUIContent(it.displayName);
                    var guiStyle = new GUIStyle(GUI.skin.label);
                    Vector2 size = guiStyle.CalcSize(guiContent);

                    HorizontalSpacings.Add(size.x);
                }
                var labelWidthOption = GUILayout.Width(HorizontalSpacings[counter]);
                counter++;

                EditorGUILayout.LabelField(it.displayName, labelWidthOption, heightOption);

                if (counter == HorizontalSpacings.Count) HorizontalSpacings.Add(50f);
                var valueWidthOption = GUILayout.Width(HorizontalSpacings[counter]);
                counter++;
                EditorGUILayout.PropertyField(it, new GUIContent(), true, valueWidthOption, heightOption);
            }
            serializedElement.ApplyModifiedProperties();
            if (GUILayout.Button("Delete")) Sheet.Delete(element);

            HorizontalSpacings.RemoveRange(counter, HorizontalSpacings.Count - counter);
        }

        // 指定間隔で垂直方向にCollectionの要素を描画する。
        public void DrawElements()
        {
            for (int i = 0; i < Sheet.Collection.Count; i++)
            {
                if (!Sheet.Collection[i]) return;
                if (i == VerticalSpacings.Count) VerticalSpacings.Add(20f);

                EditorGUILayout.BeginHorizontal();
                DrawElement(Sheet.Collection[i], VerticalSpacings[i]);
                EditorGUILayout.EndHorizontal();
            }
        }

        // グリッド状に、仕切り線を描画する。
        public void DrawGridLines()
        {
            Handles.BeginGUI();
            Handles.color = GridLineColor;

            DrawHorizontalLines();
            DrawVerticalLines();

            Handles.EndGUI();
        }

        private void DrawHorizontalLines()
        {
            var totalWidth = GridLineOffset.x;
            foreach (var width in HorizontalSpacings)
            {
                totalWidth += width + GridLineSize.x;
            }

            var y = GridLineOffset.y;
            Handles.DrawLine(new Vector2(GridLineOffset.x, y), new Vector2(totalWidth, y));

            for (int i = 0; i < Sheet.Count && i < VerticalSpacings.Count; i++)
            {
                y += VerticalSpacings[i] + GridLineSize.y;

                Handles.DrawLine(new Vector2(GridLineOffset.x, y), new Vector2(totalWidth, y));
            }
        }

        private void DrawVerticalLines()
        {
            var totalHeight = GridLineOffset.y;

            for (int i = 0; i < Sheet.Count && i < VerticalSpacings.Count; i++)
            {
                var height = VerticalSpacings[i];
                totalHeight += height + GridLineSize.y;
            }

            var x = GridLineOffset.x;
            Handles.DrawLine(new Vector2(x, GridLineOffset.y), new Vector2(x, totalHeight));

            for (int i = 0; i < HorizontalSpacings.Count; i += 2)
            {
                x += HorizontalSpacings[i] + HorizontalSpacings[i + 1] + GridLineSize.x * 2f;
                Handles.DrawLine(new Vector2(x, GridLineOffset.y), new Vector2(x, totalHeight));
            }
        }

        public void DrawValueFields()
        {
            if (!this) return;
            var serializedObject = new SerializedObject(this);

            serializedObject.Update();
            var h = serializedObject.FindProperty(nameof(HorizontalSpacings));
            for (int i = 0; i < h.arraySize; i++)
            {
                EditorGUILayout.PropertyField(h.GetArrayElementAtIndex(i), new GUIContent($"{h.displayName}: {i}"));
            }

            var v = serializedObject.FindProperty(nameof(VerticalSpacings));
            for (int i = 0; i < v.arraySize; i++)
            {
                EditorGUILayout.PropertyField(v.GetArrayElementAtIndex(i), new GUIContent($"{v.displayName}: {i}"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(GridLineColor)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(GridLineSize)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(GridLineOffset)));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif