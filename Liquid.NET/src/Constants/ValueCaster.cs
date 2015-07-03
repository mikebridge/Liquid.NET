using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Filters;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public static class ValueCaster
    {
        public static LiquidExpressionResult Cast<TSource, TDest>(TSource src)
            where TDest : IExpressionConstant
            where TSource : IExpressionConstant
        {
            if (src == null)
            {
                return LiquidExpressionResult.Success(new None<IExpressionConstant>());
            }
            if (src is TDest)
            {
                //var result = (TDest) ((dynamic) src);                
                IExpressionConstant success = (TDest)((dynamic)src);
                return LiquidExpressionResult.Success( new Some<IExpressionConstant>(success) );
            }

            return Convert<TDest>((dynamic)src);
        }
//
//        public static LiquidExpressionResult Cast<TSource, TDest>(Option<TSource> src)
//            where TDest : IExpressionConstant
//            where TSource : IExpressionConstant
//        {
//            if (!src.HasValue)
//            {
//                //new LiquidExpressionResult(result);
//                return LiquidExpressionResult.Success(Option<IExpressionConstant>.None());
//            }
//            return Cast<TSource,TDest>(src.Value);
//
//           
//        }

        private static LiquidExpressionResult Convert<TDest>(NumericValue num)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (NumericValue))
            {
                return LiquidExpressionResult.Success(num);
            }

            if (destType == typeof (StringValue))
            {
                var strResult = num.IsInt ? 
                    num.IntValue.ToString():
                    num.DecimalValue.ToString("0.0###");
                    //num.DecimalValue.ToString("G29");

                return LiquidExpressionResult.Success(new StringValue(strResult));
            }
            return LiquidExpressionResult.Success(new NumericValue(0)); // Liquid seems to convert unknowns to numeric.
            //return ConstantFactory.CreateError<TDest>("Can't convert from numeric to " + destType);
        }

        private static LiquidExpressionResult Convert<TDest>(BooleanValue boolean)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (BooleanValue))
            {
                return LiquidExpressionResult.Success(boolean);
            }

            if (destType == typeof (StringValue))
            {
                return LiquidExpressionResult.Success(new StringValue(boolean.Value.ToString().ToLower()));
            }
            return LiquidExpressionResult.Error("Can't convert from boolean to " + destType);
            //return ConstantFactory.CreateError<TDest>("Can't convert from boolean to " + destType);

        }

       
        private static LiquidExpressionResult Convert<TDest>(DictionaryValue dictionaryValue)
           where TDest : IExpressionConstant
        {
            //Console.WriteLine("Rendering dictionary");
            var destType = typeof(TDest);

            if (destType == typeof(StringValue))
            {
                //Console.WriteLine("Converting dict to string");
//                foreach (var key in dictionaryValue.DictValue.Keys)
//                {
//                    Console.WriteLine("KEY " + key + "=" + dictionaryValue.DictValue[key]);
//                }

                var result= new StringValue(
                    dictionaryValue.DictValue
                        .Keys
                        .Aggregate("", (current, key) => current + FormatKvPair(key, dictionaryValue.DictValue[key])));
                return LiquidExpressionResult.Success(result);
            }
            // So, according to https://github.com/Shopify/liquid/wiki/Liquid-for-Designers, a hash value will be iterated
            // as an array with two indices.
            if (destType == typeof (ArrayValue))
            {
                var dictarray = dictionaryValue.DictValue.Keys.Select(k =>
                {
                    var list = new List<Option<IExpressionConstant>> { new Some<IExpressionConstant>(new StringValue(k)), dictionaryValue.DictValue[k] };
                    return (Option<IExpressionConstant>) new Some<IExpressionConstant>(new ArrayValue(list));
                }).ToList();
                return LiquidExpressionResult.Success(new ArrayValue(dictarray));
            }
            // TODO: Should this return the default value for whatever TDest is requested?
            return LiquidExpressionResult.Error("Can't convert from a DictionaryValue to " + destType);
        }

        private static LiquidExpressionResult Convert<TDest>(ArrayValue arrayValue)
              where TDest : IExpressionConstant
        {
            //Console.WriteLine("Rendering array");
            var destType = typeof(TDest);

            if (destType == typeof(StringValue))
            {
                //Console.WriteLine("Converting array to string");

                return LiquidExpressionResult.Success(new StringValue(FormatArray(arrayValue)));
            }
            // TODO: Should this return the default value for whatever TDest is requested?
            return LiquidExpressionResult.Error("Can't convert from an ArrayValue to " + destType);
        }

        private static string FormatArray(ArrayValue arrayValue)
        {
            // The JSON way:
            //var strs = arrayValue.ArrValue.Select(x => Quote(GetWrappedType(x), RenderAsString(x)));
            //return "[ " + String.Join(", ", strs) + " ]"; 

            // The Concatenated way:
            var strs = arrayValue.ArrValue.Select(RenderAsString);
            return String.Join("", strs); 

        }


        private static String FormatKvPair(string key, Option<IExpressionConstant> expressionConstant)
        {
            Type wrappedType = GetWrappedType(expressionConstant);
            String exprConstantAsString = RenderAsString(expressionConstant);
            return "{ " + Quote(typeof(StringValue), key) + " : " + Quote(wrappedType, exprConstantAsString) + " }";
        }

        private static Type GetWrappedType<T>(Option<T> expressionConstant)
            where T:IExpressionConstant
        {
            if (expressionConstant.HasValue)
            {
                var nestedType = expressionConstant.Value.GetType();
                //Console.WriteLine("NEsted type " + nestedType);
                return nestedType;
            }
            else
            {
                return null;
            }
        }

        private static String Quote(Type origType, String str)
        {
            if (origType == null)
            {
                return "null";
            }
            if (origType.IsAssignableFrom(typeof(NumericValue)) || origType.IsAssignableFrom(typeof(BooleanValue)))
            {
                return str;
            }
            else
            {
                return "\"" + str + "\"";
            }
        }

        private static LiquidExpressionResult Convert<TDest>(StringValue str)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (StringValue))
            {
                return LiquidExpressionResult.Success(str);
            }

            if (destType == typeof (NumericValue))
            {
                try
                {
                    var stringVal = str.StringVal;
                    if (stringVal == null)
                    {
                        return LiquidExpressionResult.Success(new NumericValue(0));  // liquid to_numeric seems to convert these to 0.
                    }
                    if (stringVal.Contains("."))
                    {
                        var val = decimal.Parse(stringVal);

                        return LiquidExpressionResult.Success(new NumericValue(val));
                    }
                    else
                    {
                        try
                        {
                            var val = int.Parse(stringVal);
                            return LiquidExpressionResult.Success(new NumericValue(val));
                        }
                        catch (OverflowException oex)
                        {
                            var val = decimal.Parse(stringVal);

                            return LiquidExpressionResult.Success(new NumericValue(val));
                        }
                    }
                   
                    
                }
                catch
                {
                    // https://github.com/Shopify/liquid/blob/master/lib/liquid/standardfilters.rb
                    return LiquidExpressionResult.Success(new NumericValue(0));  // liquid to_numeric seems to convert these to 0.
                }
            }

            if (destType == typeof (ArrayValue))
            {
                var expressionConstants = new Some<IExpressionConstant>(str);
                // IN liquid, it doesn't seem to cast a string to an array of chars---it casts to an array of one element.
                //var expressionConstants = str.StringVal.Select(x => (Option<IExpressionConstant>) new Some<IExpressionConstant>(new StringValue(x.ToString())));
                return LiquidExpressionResult.Success(new ArrayValue(new List<Option<IExpressionConstant>>{expressionConstants}));
            }
            return LiquidExpressionResult.Error("Can't convert from string to " + destType);
           
        }


        private static LiquidExpressionResult Convert<TDest>(IExpressionConstant source)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (StringValue))
            {
                return LiquidExpressionResult.Success(new StringValue(source.ToString()));
            }
            if (destType == typeof (NumericValue))
            {
                return LiquidExpressionResult.Success(new NumericValue(0));
            }
            return LiquidExpressionResult.Error("Can't convert from " + source.GetType() + " to " + destType);

        }

        /// <summary>
        /// Use instead of Convert.ToInt32.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ConvertToInt(decimal val)
        {
            return (int) Math.Round(val, MidpointRounding.AwayFromZero);
        }

        // Not sure where to put these yet
        /// <summary>
        /// Make a list of functions, each of which has the input of the previous function.  Interpolate a casting
        /// function if the input of one doesn't fit with the value of the next.
        /// 
        /// TODO: I think this should be part of the bind function.
        /// </summary>
        /// <param name="filterExpressions"></param>
        /// <returns></returns>
        public static IEnumerable<IFilterExpression> InterpolateCastFilters(IEnumerable<IFilterExpression> filterExpressions)
        {

            var result = new List<IFilterExpression>();

            Type expectedInputType = null;
            foreach (var filterExpression in filterExpressions)
            {
                // TODO: The expectedInputType might be a superclass of the output (not just equal type)
                //if (expectedInputType != null && filterExpression.SourceType != expectedInputType)
                if (expectedInputType != null && !filterExpression.SourceType.IsAssignableFrom(expectedInputType))
                {
                    //Console.WriteLine("Creating cast from " + filterExpression + " TO " + expectedInputType);
                    result.Add(CreateCastFilter(expectedInputType, filterExpression.SourceType));
                }
                result.Add(filterExpression);
                expectedInputType = filterExpression.ResultType;
            }

            return result;
        }

        public static IFilterExpression CreateCastFilter(Type sourceType, Type resultType)
        {
            // TODO: Move this to FilterFactory.Instantiate
            Type genericClass = typeof(CastFilter<,>);
            // MakeGenericType is badly named
            //Console.WriteLine("FilterChain Creating Converter from " + sourceType + " to " + resultType);
            Type constructedClass = genericClass.MakeGenericType(sourceType, resultType);
            return (IFilterExpression)Activator.CreateInstance(constructedClass);
        }

        public static string RenderAsString(Option<IExpressionConstant> val)
        {

            return val.HasValue ? RenderAsString(val.Value) : "";

//            // TODO: Does this render an error if it can't cast or an empty string?
//            if (stringResult.IsError)
//            {
//                return "";
//            }
//            return stringResult.SuccessResult.HasValue ? ((StringValue)stringResult.SuccessResult.Value).StringVal : "";
        }

        public static string RenderAsString(IExpressionConstant val)
        {

            if (val == null)
            {
                return "";
            }

            var stringResult = Cast<IExpressionConstant, StringValue>(val);

            if (stringResult.IsError)
            {
                return "";
            }
            return stringResult.SuccessResult.HasValue ? ((StringValue)stringResult.SuccessResult.Value).StringVal : "";
        }

    }
}
