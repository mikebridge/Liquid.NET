namespace Liquid.NET.Expressions
{
    public interface ITernaryExpression<T>
    {
        T Eval(T arg1, T arg2, T arg3);
    }
}
