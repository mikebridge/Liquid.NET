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
    public class SortFilter : FilterExpression<LiquidCollection, LiquidCollection>
    {
        private readonly LiquidString _sortField;

        public SortFilter(LiquidString sortField)
        {
            _sortField = sortField;
        }

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidCollection liquidArrayExpression)
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

        private LiquidExpressionResult SortByProperty(ITemplateContext ctx, LiquidCollection val, string sortfield)
        {
            if (ctx.Options.ErrorWhenValueMissing &&
                val.Any(x => FieldAccessor.TryField(ctx, x.Value, sortfield).IsError))
            {
                return LiquidExpressionResult.Error("an array element is missing the field '" + sortfield + "'");
            }
            var ordered = val.OrderBy(x => AsString(ctx, x, sortfield));

            return LiquidExpressionResult.Success(new LiquidCollection(ordered.ToList()));
        }

        private static LiquidCollection SortAsArrayOfStrings(LiquidCollection val)
        {
            var result = val.OrderBy(ValueCaster.RenderAsString);
            return new LiquidCollection(result.ToList());
        }

        private String AsString(ITemplateContext ctx, Option<ILiquidValue> x, string field)
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
