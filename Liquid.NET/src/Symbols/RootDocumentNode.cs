namespace Liquid.NET.Symbols
{
    /// <summary>
    /// The root node in an AST document
    /// </summary>
    public class RootDocumentNode : IASTNode
    {
        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
