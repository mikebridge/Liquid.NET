using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Expressions
{
    public interface IUnaryExpression<T>
    {
        T Eval(T arg1);
    }
}
