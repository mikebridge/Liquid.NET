using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Constants
{
    [AttributeUsage(AttributeTargets.All)]
    // TODO: Maybe this isn't necessary --- have a look at filter usage other than Shopify.
    public class FilterArgumentAttribute : Attribute
    {
        public String Name { get; set; }
    }
}
