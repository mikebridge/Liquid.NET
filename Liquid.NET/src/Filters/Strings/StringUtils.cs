using System;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public static class StringUtils
    {
        public static LiquidString Eval(ILiquidValue liquidExpression, Func<String, String> f)
        {
            String before = ValueCaster.RenderAsString(liquidExpression);

            if (String.IsNullOrWhiteSpace(before))
            {
                return LiquidString.Create("");
            }
            return LiquidString.Create(f(before));
        }

        public static String ReplaceFirst(string origStr, String removeStr, String replaceStr)
        {
            var pos = origStr.IndexOf(removeStr, StringComparison.Ordinal);
            return pos < 0
                ? origStr
                : origStr.Substring(0, pos) + replaceStr + origStr.Substring(pos + removeStr.Length);
        }

    }
}
