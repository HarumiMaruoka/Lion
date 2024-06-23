using System;

namespace Glib.NovelGameEditor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RelatedModelAttribute : Attribute
    {
        public RelatedModelAttribute(Type modelType)
        {
            ModelType = modelType;
        }

        public Type ModelType { get; }
    }
}