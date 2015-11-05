using Liquid.NET.Symbols;

namespace Liquid.NET.Tags
{
    public class DecrementTag : IASTNode
    {
        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string VarName { get; set; }
    }
}
