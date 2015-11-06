using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array 
{
    public class MapFilter: FilterExpression<IExpressionConstant , IExpressionConstant>
    {
        private readonly StringValue _selector;

        public MapFilter(StringValue selector)
        {
            _selector = selector;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            //return LiquidExpressionResult.Error("Can't map that object type.  It is not an array or a hash.");
            return LiquidExpressionResult.Success(new None<IExpressionConstant>());
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ArrayValue liquidArrayExpression)
        {
            var list = liquidArrayExpression.Select(x => x.HasValue
                ? FieldAccessor.TryField(ctx, x.Value, _selector.StringVal)
                : LiquidExpressionResult.ErrorOrNone(ctx, _selector.StringVal)).ToList();
                //new None<IExpressionConstant>()).ToList();
            return list.Any(x => x.IsError) ? 
                list.First(x => x.IsError) : 
                LiquidExpressionResult.Success(new ArrayValue(list.Select(x => x.SuccessResult).ToList()));
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, DictionaryValue liquidDictionaryValue)
        {
            String propertyNameString = ValueCaster.RenderAsString((IExpressionConstant)_selector);

            return LiquidExpressionResult.Success(liquidDictionaryValue.ValueAt(propertyNameString));
            
        }

    }
}
