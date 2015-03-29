using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class Md5Filter : FilterExpression<IExpressionConstant, StringValue>
    {

        public override StringValue ApplyTo(IExpressionConstant liquidExpression)
        {
            return StringUtils.Eval(liquidExpression, ToMd5);
            
        }

        public String ToMd5(String s)
        {
            byte[] hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(s));
            return hash.Aggregate("", (current, t) => current + t.ToString("X2")).ToLower();
        }


    }
}
