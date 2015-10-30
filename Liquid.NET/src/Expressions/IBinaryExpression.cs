namespace Liquid.NET.Expressions
{
    public interface IBinaryExpression<T>
    {
        T Eval(T arg1, T arg2);
    }
}
