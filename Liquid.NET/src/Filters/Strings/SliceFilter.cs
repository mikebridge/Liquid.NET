using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#slice
    /// Note: documentation is wrong: negative start index starts at -1.
    /// (as per ruby http://ruby-doc.org/core-2.2.0/Array.html)
    /// Verified in liquid.
    /// </summary>
    public class SliceFilter : FilterExpression<IExpressionConstant, IExpressionConstant>
    {
        private readonly NumericValue _start;
        private readonly NumericValue _length;

        public SliceFilter(NumericValue start, NumericValue length)
        {
            _start = start;
            _length = length;
        }

        public override IExpressionConstant ApplyTo(IExpressionConstant objectExpression)
        {
            return ConstantFactory.CreateError<ArrayValue>("Can't find sub-elements from that object.  It is not an array or a string.");
        }

        public override IExpressionConstant ApplyTo(ArrayValue originalArrayValue)
        {
            if (originalArrayValue == null || originalArrayValue.Value == null)
            {
                return ConstantFactory.CreateError<ArrayValue>("Array is nil");
            }
            var list = originalArrayValue.ArrValue;

            if (_start == null || _start.IsUndefined)
            {
                return ConstantFactory.CreateError<ArrayValue>("Please pass a start parameter.");
            }

            return new ArrayValue(SliceList(list));
        }

        public override IExpressionConstant ApplyTo(StringValue stringValue)
        {
            if (stringValue == null || stringValue.Value == null)
            {
                return ConstantFactory.CreateError<StringValue>("String is null");
            }
            var list = stringValue.StringVal.ToCharArray().ToList();

            if (_start == null || _start.IsUndefined)
            {
                return ConstantFactory.CreateError<StringValue>("Please pass a start parameter.");
            }

            return new StringValue(String.Concat(SliceList(list)));
        }

        private IList<T> SliceList<T>(IList<T> list)
        {
            var take = 1;
            var skip = _start.IntValue;
            skip = FindPosition(list, skip);

            if (_length != null && !_length.IsUndefined)
            {
                take = _length.IntValue;
            }
            if (take < 0)
            {
                return new List<T>();
            }

            return Slice(list, skip, take);

        }

        /// <summary>
        /// Negative counts frome end.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        private static int FindPosition<T>(IList<T> list, int skip)
        {
            if (skip < 0)
            {
                skip = list.Count + skip;
            }
            return skip;
        }


        private static IList<T> Slice<T>(IList<T> list, int skip, int take)
        {            
            return new List<T>(list.Skip(skip).Take(take));            
        }

    }
}
