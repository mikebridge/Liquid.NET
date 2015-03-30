using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Symbols;

namespace Liquid.NET.Tags
{
    public class BreakTag : IASTNode
    {
        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
