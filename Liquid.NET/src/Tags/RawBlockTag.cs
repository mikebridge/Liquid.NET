using System;
using Liquid.NET.Symbols;

namespace Liquid.NET.Tags
{
    public class RawBlockTag : IASTNode
    {
        private readonly string _str;

        public RawBlockTag(String str)
        {
            _str = str;
        }

        public String Value { get { return _str;  } }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
