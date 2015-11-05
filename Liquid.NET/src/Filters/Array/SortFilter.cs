using System;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Array
{

    /// <summary>
    /// Sort an array based on the raw "rendered" value of the string.
    /// TODO: specify an expression as a filter to use to sort, instead
    ///  of the default "render as string".
    /// </summary>
    public class SortFilter : FilterExpression<ArrayValue, ArrayValue>
    {
        private readonly StringValue _sortField;

        public SortFilter(StringValue sortField)
        {
            _sortField = sortField;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, ArrayValue liquidArrayExpression)
        {            
            var sortfield = _sortField.StringVal;

            if (String.IsNullOrWhiteSpace(sortfield)) // try to sort as an array
            {
                return LiquidExpressionResult.Success(SortAsArrayOfStrings(liquidArrayExpression));
            }
            else
            {
                return SortByProperty(ctx, liquidArrayExpression, sortfield);
            }
        }

        private LiquidExpressionResult SortByProperty(ITemplateContext ctx, ArrayValue val, string sortfield)
        {
            if (ctx.Options.ErrorWhenValueMissing &&
                val.ArrValue.Any(x => FieldAccessor.TryField(ctx, x.Value, sortfield).IsError))
            {
                return LiquidExpressionResult.Error("an array element is missing the field '" + sortfield + "'");
            }
            var ordered = val.ArrValue.OrderBy(x => AsString(ctx, x, sortfield));

            return LiquidExpressionResult.Success(new ArrayValue(ordered.ToList()));
        }

        private static ArrayValue SortAsArrayOfStrings(ArrayValue val)
        {
            var result = val.ArrValue.OrderBy(ValueCaster.RenderAsString);
            return new ArrayValue(result.ToList());
        }

        private String AsString(ITemplateContext ctx, Option<IExpressionConstant> x, string field)
        {
            if (!x.HasValue)
            {
                return "";
            }

            var liquidExpressionResult = FieldAccessor.TryField(ctx, x.Value, field);
            if (liquidExpressionResult.IsError || !liquidExpressionResult.SuccessResult.HasValue)
            {
                return "";
            }
            return ValueCaster.RenderAsString(liquidExpressionResult.SuccessResult.Value);
        }
    }



}
