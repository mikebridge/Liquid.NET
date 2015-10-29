namespace Liquid.NET.Constants
{
    public interface IIndexable
    {
        IExpressionConstant ValueAt(IExpressionConstant key);
    }
}
