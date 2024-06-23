#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace Glib.NovelGameEditor
{
    public interface IOutputNodeView : INodeView
    {
#if UNITY_EDITOR
        Port OutputPort { get; }
#endif
        IOutputNode OutputConnectable { get; }
    }
}