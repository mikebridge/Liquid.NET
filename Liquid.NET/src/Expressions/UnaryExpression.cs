namespace Liquid.NET.Expressions
{
    public interface IUnaryExpression<T>
    {
        T Eval(T arg1);
    }
}
