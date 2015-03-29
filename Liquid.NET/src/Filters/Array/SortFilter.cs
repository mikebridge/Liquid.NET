using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

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

        public override ArrayValue ApplyTo(ArrayValue liquidArrayExpression)
        {            
            var sortfield = _sortField.StringVal;

            if (String.IsNullOrWhiteSpace(sortfield)) // try to sort as an array
            {
                return SortAsArrayOfStrings(liquidArrayExpression);
            }
            else
            {
                return SortByProperty(liquidArrayExpression, sortfield);
            }
        }

        private ArrayValue SortByProperty(ArrayValue val, string sortfield)
        {
            var ordered = val.ArrValue.OrderBy(x => AsString(x, sortfield));
            // TODO: ThenBy
            return new ArrayValue(ordered.ToList());
        }

        private static ArrayValue SortAsArrayOfStrings(ArrayValue val)
        {
            var result = val.ArrValue.OrderBy(ValueCaster.RenderAsString);
            return new ArrayValue(result.ToList());
        }

        private String AsString(IExpressionConstant x, string field)
        {
            return ValueCaster.RenderAsString(FieldAccessor.TryField(x, field));
        }
    }



}
