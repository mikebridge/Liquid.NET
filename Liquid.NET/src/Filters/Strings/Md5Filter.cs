using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Md5Filter : FilterExpression<IExpressionConstant, StringValue>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, ToMd5));
            
        }

        public String ToMd5(String s)
        {
            byte[] hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(s));
            return hash.Aggregate("", (current, t) => current + t.ToString("X2")).ToLower();
        }


    }
}
