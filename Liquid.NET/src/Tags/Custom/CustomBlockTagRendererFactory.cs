using System;

namespace Liquid.NET.Tags.Custom
{
    public static class CustomBlockTagRendererFactory
    {
        public static ICustomBlockTagRenderer Create(Type tagType)
        {
            if (tagType == null)
            {
                return null;
            }
            return (ICustomBlockTagRenderer) Activator.CreateInstance(tagType);
        }
    }
}
