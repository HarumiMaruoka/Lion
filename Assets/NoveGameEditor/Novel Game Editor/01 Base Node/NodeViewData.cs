using UnityEngine;
using System;

namespace Glib.NovelGameEditor
{
    [Serializable]
    public class NodeViewData
    {
        [SerializeField]
        private string _guid;
        [SerializeField]
        public Vector2 _position;
        [SerializeField]
        private string _title;

        public string GUID
        {
            get
            {
#if UNITY_EDITOR
                if (string.IsNullOrEmpty(_guid))
                {
                    _guid = UnityEditor.GUID.Generate().ToString();
                }
#endif

                return _guid;
            }
        }
        public Vector2 Position { get => _position; set => _position = value; }
        public string Title { get => _title; set => _title = value; }
    }
}