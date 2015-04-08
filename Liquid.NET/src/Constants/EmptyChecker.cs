using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Grammar;

namespace Liquid.NET.Constants
{
    public static class EmptyChecker
    {
        public static bool IsEmpty(IExpressionConstant val)
        {
            if (val == null || val.IsUndefined)
            {
                return false; // this appears to be the case in liquid?
            }
            return CheckIsEmpty((dynamic)val);
        }

        private static bool CheckIsEmpty(IExpressionConstant val)
        {
            return false; // the only conditions will have been caught by IsEmpty
        }

        private static bool CheckIsEmpty(StringValue val)
        {
            return String.IsNullOrEmpty(val.StringVal);
        }


        private static bool CheckIsEmpty(ArrayValue val)
        {
            return !val.ArrValue.Any();
        }

        private static bool CheckIsEmpty(DictionaryValue val)
        {
            return !val.DictValue.Any();
        }


    }
}
