using System;
using UnityEngine;
using UnityEditor;

namespace Lion.GameDataSheet
{
    public class PathFinder : MonoBehaviour
    {
        public static string GetProjectPath()
        {
            return Application.dataPath;
        }
    }
}