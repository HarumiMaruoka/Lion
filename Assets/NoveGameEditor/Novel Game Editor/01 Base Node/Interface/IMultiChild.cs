using System.Collections.Generic;

namespace Glib.NovelGameEditor
{
    public interface IMultiChild : IOutputNode
    {
        List<Node> Children { get; }
    }
}