using System;

namespace Liquid.NET.Tags.Custom
{
    public static class CustomTagRendererFactory
    {
        public static ICustomTagRenderer Create(Type tagType)
        {
            if (tagType == null)
            {
                return null;
            }
            //Console.WriteLine("Instantiating " + tagType);
            return (ICustomTagRenderer) Activator.CreateInstance(tagType);
        }
    }
}
