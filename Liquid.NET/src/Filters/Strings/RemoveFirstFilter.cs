using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RemoveFirstFilter : FilterExpression<IExpressionConstant, StringValue>
    {
        private readonly StringValue _stringToRemove;

        public RemoveFirstFilter(StringValue stringToRemove)
        {
            _stringToRemove = stringToRemove;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => StringUtils.ReplaceFirst(x, _stringToRemove.StringVal, "")));            
        }

       
    }
}
