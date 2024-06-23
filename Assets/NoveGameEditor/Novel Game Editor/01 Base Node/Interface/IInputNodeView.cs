#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace Glib.NovelGameEditor
{
    public interface IInputNodeView : INodeView
    {
#if UNITY_EDITOR
        Port InputPort { get; }
#endif
        IInputNode InputConnectable { get; }
    }
}