
namespace Glib.NovelGameEditor
{
    public interface ISingleParent : IInputNode
    {
        Node Parent { get; set; }
    }
}