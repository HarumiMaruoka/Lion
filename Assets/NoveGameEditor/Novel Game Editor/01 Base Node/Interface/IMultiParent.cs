using System.Collections.Generic;

namespace Glib.NovelGameEditor
{
    public interface IMultiParent : IInputNode
    {
        List<Node> Parents { get; }
    }
}