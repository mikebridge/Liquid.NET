using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine("Instantiating " + tagType);
            //(ICustomTagRenderer)Activator.CreateInstance(tagType, args);
            return (ICustomTagRenderer) Activator.CreateInstance(tagType);
        }
    }
}
