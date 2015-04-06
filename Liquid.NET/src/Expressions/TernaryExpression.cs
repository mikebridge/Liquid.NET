using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Expressions
{
    public interface ITernaryExpression<T>
    {
        T Eval(T arg1, T arg2, T arg3);
    }
}
