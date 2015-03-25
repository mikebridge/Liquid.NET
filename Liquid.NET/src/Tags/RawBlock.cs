using System;
using Liquid.NET.Symbols;

namespace Liquid.NET.Tags
{
    public class RawBlock : IASTNode
    {
        private readonly string _str;

        public RawBlock(String str)
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
