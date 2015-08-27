using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public class IndexDereferencer
    {
        /// <summary>
        /// Look up the index in the value.  This works for dictionaries, arrays and strings.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="value"></param>
        /// <param name="indexProperty"></param>
        /// <returns></returns>
        public LiquidExpressionResult Lookup(
            ITemplateContext ctx, 
            IExpressionConstant value,
            IExpressionConstant indexProperty)
        {
            Console.WriteLine("LOOKUP=> VALUE: " + value);
            Console.WriteLine("      => INDEX: " + indexProperty);
            return DoLookup(ctx, (dynamic) value, indexProperty);
        }

        private LiquidExpressionResult DoLookup(ITemplateContext ctx, ArrayValue arrayValue, IExpressionConstant indexProperty)
        {

            String propertyNameString = ValueCaster.RenderAsString(indexProperty);
            int index;
            if (propertyNameString.ToLower().Equals("first"))
            {
                index = 0;
            }
            else if (propertyNameString.ToLower().Equals("last"))
            {
                index = arrayValue.ArrValue.Count - 1;
            }
            else if (propertyNameString.ToLower().Equals("size"))
            {
                return LiquidExpressionResult.Success(new NumericValue(arrayValue.ArrValue.Count));
            }
            else
            {
                var maybeIndexResult = ValueCaster.Cast<IExpressionConstant, NumericValue>(indexProperty);
                if (maybeIndexResult.IsError || !maybeIndexResult.SuccessResult.HasValue)
                {
                    return LiquidExpressionResult.Error("invalid array index: " + propertyNameString);
                }
                else
                {
                    index = maybeIndexResult.SuccessValue<NumericValue>().IntValue;
                }
            }

            if (arrayValue.ArrValue.Count == 0)
            {
                //return LiquidExpressionResult.Error("array is empty: " + propertyNameString);
                return LiquidExpressionResult.Success(new None<IExpressionConstant>()); // not an error in Ruby liquid.
            }
            var result = arrayValue.ValueAt(index);
            return LiquidExpressionResult.Success(result);
        }

        private LiquidExpressionResult DoLookup(ITemplateContext ctx, DictionaryValue dictionaryValue, IExpressionConstant indexProperty)
        {

            String propertyNameString = ValueCaster.RenderAsString(indexProperty);
            if (propertyNameString.ToLower().Equals("size"))
            {
                return LiquidExpressionResult.Success(new NumericValue(dictionaryValue.DictValue.Keys.Count()));
            }

            return LiquidExpressionResult.Success(dictionaryValue.ValueAt(indexProperty.Value.ToString()));
        }

        // TODO: this is inefficient and ugly and duplicates much of ArrayValue
        private LiquidExpressionResult DoLookup(ITemplateContext ctx, StringValue strValue, IExpressionConstant indexProperty)
        {
            var strValues = strValue.StringVal.ToCharArray().Select(ch => new StringValue(ch.ToString()).ToOption()).ToList();
            String propertyNameString = ValueCaster.RenderAsString(indexProperty);
            int index;
            if (propertyNameString.ToLower().Equals("first"))
            {
                index = 0;
            }
            else if (propertyNameString.ToLower().Equals("last"))
            {
                index = strValues.Count - 1;
            }
            else if (propertyNameString.ToLower().Equals("size"))
            {
                return LiquidExpressionResult.Success(new NumericValue(strValues.Count));
            }
            else
            {
                var maybeIndexResult = ValueCaster.Cast<IExpressionConstant, NumericValue>(indexProperty);
                if (maybeIndexResult.IsError || !maybeIndexResult.SuccessResult.HasValue)
                {
                    return LiquidExpressionResult.Error("invalid array index: " + propertyNameString);
                }
                else
                {
                    index = maybeIndexResult.SuccessValue<NumericValue>().IntValue;
                }
            }

            if (strValues.Count == 0)
            {
                //return LiquidExpressionResult.Error("Empty string: " + propertyNameString);
                return LiquidExpressionResult.Success(new None<IExpressionConstant>()); // not an error in Ruby liquid.
            }
            return LiquidExpressionResult.Success(ArrayIndexer.ValueAt(strValues, index));

        }

    }
}
