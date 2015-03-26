using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Array
{
    public class SortFilter : FilterExpression<ArrayValue, ArrayValue>
    {
        private readonly StringValue _sortField;

        public SortFilter(StringValue sortField)
        {
            _sortField = sortField;
        }

        public override ArrayValue ApplyTo(ArrayValue val)
        {
            return val;
        }

        public override ArrayValue ApplyTo(DictionaryValue val)
        {
            var dict = val.DictValue;
            return dict.Keys.Select(key => dict[key]).Sort()
        }
    }



}
