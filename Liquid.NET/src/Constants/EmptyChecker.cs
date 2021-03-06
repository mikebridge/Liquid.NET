﻿using System;
using System.Linq;

namespace Liquid.NET.Constants
{
    public static class EmptyChecker
    {
        public static bool IsEmpty(ILiquidValue val)
        {
            if (val == null)
            {
                return false;
            }
        
            var str = val as LiquidString;
            if (str != null)
            {
                return CheckIsEmpty(str);
            }

            var arr = val as LiquidCollection;
            if (arr != null)
            {
                return CheckIsEmpty(arr);
            }

            var dict = val as LiquidHash;
            if (dict != null)
            {
                return CheckIsEmpty(dict);
            }

            return CheckIsEmpty(val);
        }

        // ReSharper disable once UnusedParameter.Local
        private static bool CheckIsEmpty(ILiquidValue _)
        {
            return false; // the only conditions will have been caught by IsEmpty
        }

        private static bool CheckIsEmpty(LiquidString val)
        {
            return String.IsNullOrEmpty(val.StringVal);
        }


        private static bool CheckIsEmpty(LiquidCollection val)
        {
            return !val.Any();
        }

        private static bool CheckIsEmpty(LiquidHash val)
        {
            return !val.Any();
        }


    }

    public static class BlankChecker
    {
        public static bool IsBlank(ILiquidValue val)
        {
            if (val == null)
            {
                return true; // regardless of what shopify liquid + activesupport do
            }
            //return CheckIsBlank((dynamic)val);
            var str = val as LiquidString;
            if (str != null)
            {
                return CheckIsBlank(str);
            }
            var arr = val as LiquidCollection;
            if (arr != null)
            {
                return CheckIsBlank(arr);
            }
            var dict = val as LiquidHash;
            if (dict != null)
            {
                return CheckIsBlank(dict);
            }
            return CheckIsBlank(val);
        }

        // ReSharper disable once UnusedParameter.Local
        private static bool CheckIsBlank(ILiquidValue _)
        {
            return false; // the only conditions will have been caught by IsBlank -- other types are never blank
        }

        private static bool CheckIsBlank(LiquidString val)
        {
            String stringVal = val.StringVal;
            return stringVal == null || String.IsNullOrEmpty(stringVal.Trim());
        }


        private static bool CheckIsBlank(LiquidCollection val)
        {
            return !val.Any();
        }

        private static bool CheckIsBlank(LiquidHash val)
        {
            return !val.Any();
        }


    }

}
