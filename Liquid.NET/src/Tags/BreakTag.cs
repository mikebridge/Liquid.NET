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
