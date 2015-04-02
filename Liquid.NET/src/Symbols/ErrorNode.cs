using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Symbols
{
    public class ErrorNode : IASTNode
    {
        public LiquidError LiquidError { get; private set; }

        public ErrorNode(LiquidError liquidError)
        {
            LiquidError = liquidError;
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
