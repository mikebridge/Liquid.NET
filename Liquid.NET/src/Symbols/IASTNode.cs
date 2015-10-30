namespace Liquid.NET.Symbols
{
    public interface IASTNode
    {
        void Accept(IASTVisitor visitor);
    }
}
