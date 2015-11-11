using System;

namespace Liquid.NET.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LiquidIgnoreAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LiquidIgnoreIfNullAttribute : Attribute
    {
    }


    [AttributeUsage(AttributeTargets.Property)]
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
