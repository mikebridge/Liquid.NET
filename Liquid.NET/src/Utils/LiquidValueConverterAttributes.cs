using System;

namespace Liquid.NET.Utils
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LiquidIgnoreAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LiquidNameAttribute : Attribute
    {
        private readonly string _key;
        public String Key { get { return _key; }}

        public LiquidNameAttribute(String key)
        {
            _key = key;
        }
    }

}
