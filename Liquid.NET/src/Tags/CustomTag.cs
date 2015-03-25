using Liquid.NET.Symbols;

namespace Liquid.NET.Tags
{
    public class CustomTag : IASTNode
    {
        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
