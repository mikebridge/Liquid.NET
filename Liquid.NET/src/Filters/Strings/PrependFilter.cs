using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PrependFilter : FilterExpression<IExpressionConstant, LiquidString>
    {
        private readonly LiquidString _prependedStr;

        public PrependFilter(LiquidString prependedStr)
        {
            _prependedStr = prependedStr;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            var strToPrepend = _prependedStr == null ? "" : _prependedStr.StringVal;
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => strToPrepend + x));
        }
    }
}
