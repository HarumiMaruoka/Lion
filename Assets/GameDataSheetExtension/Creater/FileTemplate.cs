using System;
using UnityEngine;


namespace Lion.GameDataSheet
{
    public static class FileTemplate
    {
        public static string Data(string dataName) =>
            $"using System;                              \r\n" +
            $"using UnityEngine;                         \r\n" +
            $"                                           \r\n" +
            $"public class {dataName} : ScriptableObject \r\n" +
            $"{{                                         \r\n" +
            $"                                           \r\n" +
            $"}}                                         ";

        public static string Sheet(string sheetName, string dataName, string windowName) =>
            $"using System;                                                              \r\n" +
            $"using UnityEngine;                                                         \r\n" +
            $"                                                                           \r\n" +
            $"[CreateAssetMenu(                                                          \r\n" +
            $"    fileName = \"{sheetName}\",                                            \r\n" +
            $"    menuName = \"Game Data Sheets/{sheetName}\")]                          \r\n" +
            $"public class {sheetName} : Lion.GameDataSheet.SheetBase<{dataName}> {{ }}  \r\n" +
            $"                                                                           \r\n" +
            $"                                                                           \r\n" +
            $"#if UNITY_EDITOR                                                           \r\n" +
            $"[UnityEditor.CustomEditor(typeof({sheetName}))]                            \r\n" +
            $"public class {sheetName}Drawer : UnityEditor.Editor                        \r\n" +
            $"{{                                                                         \r\n" +
            $"    public override void OnInspectorGUI()                                  \r\n" +
            $"    {{                                                                     \r\n" +
            $"        if (GUILayout.Button(\"Open Window\"))                             \r\n" +
            $"        {{                                                                 \r\n" +
            $"            {windowName}.Init();                                           \r\n" +
            $"        }}                                                                 \r\n" +
            $"                                                                           \r\n" +
            $"        base.OnInspectorGUI();                                             \r\n" +
            $"    }}                                                                     \r\n" +
            $"}}                                                                         \r\n" +
            $"#endif                                                                         ";

        public static string WindowLayout(string layoutName, string dataName) =>
            $"#if UNITY_EDITOR                                                              \r\n" +
            $"using System;                                                                 \r\n" +
            $"using UnityEngine;                                                            \r\n" +
            $"                                                                              \r\n" +
            $"public class {layoutName} : Lion.GameDataSheet.WindowLayout<{dataName}> {{ }} \r\n" +
            $"#endif                                                                            ";

        public static string Window(string windowName, string dataName, string sheetName, string layoutName) =>
            $"#if UNITY_EDITOR                                                                                      \r\n" +
            $"using System;                                                                                         \r\n" +
            $"using UnityEditor;                                                                                    \r\n" +
            $"using UnityEngine;                                                                                    \r\n" +
            $"                                                                                                      \r\n" +
            $"public class {windowName} : Lion.GameDataSheet.SheetWindowBase<{dataName}, {sheetName}, {layoutName}> \r\n" +
            $"{{                                                                                                    \r\n" +
            $"    [MenuItem(\"Window/Game Data Sheet/{windowName}\")]                                               \r\n" +
            $"    public static void Init()                                                                         \r\n" +
            $"    {{                                                                                                \r\n" +
            $"        GetWindow(typeof({windowName})).Show();                                                       \r\n" +
            $"    }}                                                                                                \r\n" +
            $"}}                                                                                                    \r\n" +
            $"#endif                                                                                                    ";
    }
}