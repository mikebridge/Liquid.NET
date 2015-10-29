using System;

namespace Liquid.NET.Constants
{
    [AttributeUsage(AttributeTargets.All)]
    // TODO: Maybe this isn't necessary --- have a look at filter usage other than Shopify.
    public class FilterArgumentAttribute : Attribute
    {
        public String Name { get; set; }
    }
}
