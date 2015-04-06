using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Expressions
{
    public interface IBinaryExpression<T>
    {
        T Eval(T arg1, T arg2);
    }
}
