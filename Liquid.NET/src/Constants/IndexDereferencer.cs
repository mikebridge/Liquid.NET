using System;
using System.Linq;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public class IndexDereferencer
    {
        /// <summary>
        /// Look up the index in the value.  This works for dictionaries, arrays and strings.
        /// 
        /// Returns an error if errorWhenValueMissing is true.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="indexProperty"></param>
        /// <param name="errorWhenValueMissing"></param>
        /// <returns></returns>
        public LiquidExpressionResult Lookup(ILiquidValue value,
            ILiquidValue indexProperty, bool errorWhenValueMissing)
        {
            var arr = value as LiquidCollection;
            if (arr != null)
            {
                return DoLookup(arr, indexProperty, errorWhenValueMissing);
            }

            var dict = value as LiquidHash;
            if (dict != null)
            {
                return DoLookup(dict, indexProperty, errorWhenValueMissing);
            }

            var str = value as LiquidString;
            if (str != null)
            {
                return DoLookup(str, indexProperty, errorWhenValueMissing);
            }

            return LiquidExpressionResult.Error("ERROR : cannot apply an index to a " + value.LiquidTypeName + ".");
        }

        private LiquidExpressionResult DoLookup(LiquidCollection liquidCollection, ILiquidValue indexProperty, bool errorWhenValueMissing)
        {
            bool errorOnEmpty = errorWhenValueMissing && liquidCollection.Count == 0;

                            

            String propertyNameString = ValueCaster.RenderAsString(indexProperty);
            int index;
            if (propertyNameString.ToLower().Equals("first"))
            {
                if (errorOnEmpty)
                {
                    return LiquidExpressionResult.Error("cannot dereference empty array");
                }
                index = 0;
            }
            else if (propertyNameString.ToLower().Equals("last"))
            {
                if (errorOnEmpty)
                {
                    return LiquidExpressionResult.Error("cannot dereference empty array");
                }
                index = liquidCollection.Count - 1;
            }
            else if (propertyNameString.ToLower().Equals("size"))
            {
                return LiquidExpressionResult.Success(LiquidNumeric.Create(liquidCollection.Count));
            }
            else
            {
                var success = Int32.TryParse(propertyNameString, out index);
                //var maybeIndexResult = ValueCaster.Cast<ILiquidValue, LiquidNumeric>(indexProperty);

                if (!success)
                {
                    if (errorWhenValueMissing)
                    {
                        return LiquidExpressionResult.Error("invalid index: '" + propertyNameString + "'");
                    }
                    else
                    {
                        return LiquidExpressionResult.Success(new None<ILiquidValue>());// liquid seems to return nothing when non-int index.
                    }
                }

//                if (maybeIndexResult.IsError || !maybeIndexResult.SuccessResult.HasValue)
//                {
//                    return LiquidExpressionResult.Error("invalid array index: " + propertyNameString);
//                }
//                else
//                {
//                    index = maybeIndexResult.SuccessValue<LiquidNumeric>().IntValue;
//                }
            }

            if (liquidCollection.Count == 0)
            {
                return errorOnEmpty ? 
                    LiquidExpressionResult.Error("cannot dereference empty array") : 
                    LiquidExpressionResult.Success(new None<ILiquidValue>());
            }
            var result = liquidCollection.ValueAt(index);

            return LiquidExpressionResult.Success(result);
        }

        private LiquidExpressionResult DoLookup(LiquidHash liquidHash, ILiquidValue indexProperty, bool errorWhenValueMissing)
        {

            String propertyNameString = ValueCaster.RenderAsString(indexProperty);
            if (propertyNameString.ToLower().Equals("size"))
            {
                return LiquidExpressionResult.Success(LiquidNumeric.Create(liquidHash.Keys.Count));
            }

            var valueAt = liquidHash.ValueAt(indexProperty.Value.ToString());
            if (valueAt.HasValue)
            {
                return LiquidExpressionResult.Success(valueAt);
            }
            else
            {
                return LiquidExpressionResult.ErrorOrNone(indexProperty.ToString(), errorWhenValueMissing);

            }
        }

        // TODO: this is inefficient and ugly and duplicates much of LiquidCollection
        private LiquidExpressionResult DoLookup(LiquidString str, ILiquidValue indexProperty, bool errorWhenValueMissing)
        {
            var strValues = str.StringVal.ToCharArray().Select(ch => LiquidString.Create(ch.ToString()).ToOption()).ToList();
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
                return LiquidExpressionResult.Success(LiquidNumeric.Create(strValues.Count));
            }
            else
            {
                //var maybeIndexResult = ValueCaster.Cast<ILiquidValue, LiquidNumeric>(indexProperty);
                var numericIndexProperty = indexProperty as LiquidNumeric;
                
                if (numericIndexProperty == null)
                {                  
                    return errorWhenValueMissing ? 
                        LiquidExpressionResult.Error("invalid string index: '" + propertyNameString + "'") : 
                        LiquidExpressionResult.Success(new None<ILiquidValue>());
                }
                else
                {
                    index = numericIndexProperty.IntValue;
                }
            }

            if (strValues.Count == 0)
            {
                //return LiquidExpressionResult.Error("Empty string: " + propertyNameString);
                return LiquidExpressionResult.Success(new None<ILiquidValue>()); // not an error in Ruby liquid.
            }
            return LiquidExpressionResult.Success(CollectionIndexer.ValueAt(strValues, index));

        }

    }
}
